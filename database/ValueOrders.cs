using Dapper;
using data;
using database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Production_planning
{
    public class ValueOrders
    {
        public ValueOrders()
        {

        }

        public async Task<List<ProductionJobCard>> GetProductionJobsAsync(DateTime startDate, DateTime endDate, string externalConnString)
        {
            // Текст SQL-запроса из Элемента 1
            string sql = @"SELECT
                  man_planjob.id_man_planjob AS IdPlanJob,
                  man_planjob.date_begin AS DateBegin,
                  man_planjob.date_end AS DateEnd,
                  man_planjob.id_equip AS IdEquip,
                  man_planjob_list.id_norm_operation AS IdNormOperation,
                  norm_operation_table.ord AS Ord,
                  COALESCE(man_planjob_list.plan_out_qty, 0) AS PlanOutQty,
                  COALESCE(man_planjob_list.normtime, 0) AS NormTime,
                  COALESCE(order_head.order_num, '') AS OrderNum,
                  COALESCE(order_head.order_name, '') AS OrderName,
                  COALESCE(common_ul_directory.ul_name, '') AS UlName,
                  COALESCE(idletime_directory.idletime_name, '') AS IdleTimeName
                FROM dbo.man_planjob
                LEFT JOIN dbo.man_planjob_list ON man_planjob.id_man_order_job_item = man_planjob_list.id_man_order_job_item
                LEFT JOIN dbo.man_order_job_item ON man_planjob.id_man_order_job_item = man_order_job_item.id_man_order_job_item
                LEFT JOIN dbo.man_order_job ON man_order_job_item.id_man_order_job = man_order_job.id_man_order_job
                LEFT JOIN dbo.order_head ON man_order_job.id_order_head = order_head.id_order_head
                LEFT JOIN dbo.common_ul_directory ON order_head.id_customer = common_ul_directory.id_common_ul_directory
                LEFT JOIN dbo.man_idletime ON man_order_job.id_man_order_job = man_idletime.id_man_order_job
                LEFT JOIN dbo.idletime_directory ON man_idletime.id_idletime = idletime_directory.id_idletime_directory
                LEFT JOIN dbo.norm_operation_table ON man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation 
                WHERE man_planjob.status <> 2 
                  AND man_planjob.flags <> 1 
                  AND man_planjob.id_equip is not NULL
                  AND man_planjob.date_begin <= @EndDate 
                  AND man_planjob.date_end >= @StartDate
                ORDER BY man_planjob.date_begin ASC;
                ";

            // Жесткий лимит ожидания ответа от удаленной сети — 4 секунды
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4)))
            {
                try
                {
                    using (var conn = new SqlConnection(externalConnString))
                    {
                        await conn.OpenAsync(cts.Token);

                        // Качаем сырой плоский поток строк асинхронно
                        var rawRows = await conn.QueryAsync(new CommandDefinition(
                            sql,
                            new { StartDate = startDate.Date, EndDate = endDate.Date },
                            cancellationToken: cts.Token
                        ));

                        // ИНТЕЛЛЕКТУАЛЬНАЯ СБОРКА И СКЛЕЙКА ФАЗ
                        var structuredJobs = rawRows
                            .GroupBy(r => r.Ord == null
                                ? $"IDLE_{r.IdPlanJob}_{r.DateBegin:yyyyMMdd}_{r.IdEquip}" // -- Уникальный ключ для простоев
                                : $"JOB_{r.IdPlanJob}_{r.IdEquip}")                        // -- Уникальный ключ для технологических заданий
                            .Select(group =>
                            {
                                var baseRow = group.First();

                                var card = new ProductionJobCard
                                {
                                    IdPlanJob = (long)baseRow.IdPlanJob,
                                    DateBegin = (DateTime)baseRow.DateBegin,
                                    DateEnd = (DateTime)baseRow.DateEnd,
                                    IdEquip = (int)baseRow.IdEquip,
                                    OrderNum = (string)baseRow.OrderNum,
                                    OrderName = (string)baseRow.OrderName,
                                    UlName = (string)baseRow.UlName,
                                    IsIdleTime = baseRow.Ord == null,
                                    IdleTimeName = (string)baseRow.IdleTimeName,

                                    SetupCount = 0,
                                    SetupNormTime = 0,
                                    RunNormTime = 0,
                                    RunQty = 0
                                };

                                if (!card.IsIdleTime)
                                {
                                    // Фаза 1: Приладка (ord = 0). Может отсутствовать при печати на потоке!
                                    var setupRow = group.FirstOrDefault(r => r.Ord == 0);
                                    if (setupRow != null)
                                    {
                                        card.SetupCount = (double)setupRow.PlanOutQty;
                                        // 🌟 Ваше правило: делим normtime на plan_out_qty только для приладки
                                        card.SetupNormTime = (double)setupRow.PlanOutQty > 0
                                            ? (double)setupRow.NormTime / (double)setupRow.PlanOutQty
                                            : 0;
                                    }

                                    // Фаза 2: Выполнение (ord = 1)
                                    var runRow = group.FirstOrDefault(r => r.Ord == 1);
                                    if (runRow != null)
                                    {
                                        card.RunQty = (double)runRow.PlanOutQty;
                                        card.RunNormTime = (double)runRow.NormTime; // Время выполнения НЕ делим
                                    }
                                }

                                return card;
                            })
                            .ToList();

                        return structuredJobs;
                    }
                }
                catch (Exception ex)
                {
                    // Если сеть лежит или сработал таймаут 4 сек — мягко возвращаем null.
                    // Приложение не упадет, а графики людей загрузятся штатно!
                    Console.WriteLine("ERROR: GetProductionJobsAsync " + ex.Message);
                    return null;
                }
            }
        }

        public List<OrdersLoad> GetPlan(int idMachine, CancellationToken token)
        {
            List<OrdersLoad> orders = new List<OrdersLoad>();
            int lastItemIndex = -1;

            try
            {
                using (SqlConnection Connect = DBConnection.GetDBConnection())
                {
                    Connect.Open();
                    SqlCommand Command = new SqlCommand
                    {
                        Connection = Connect,
                        CommandText = @"SELECT
	                                        man_planjob.id_man_planjob, 
	                                        man_planjob.date_begin, 
	                                        man_planjob.date_end, 
	                                        man_planjob.status, 
	                                        man_planjob.flags, 
	                                        man_planjob.id_equip, 
	                                        man_planjob_list.plan_out_qty, 
	                                        man_planjob_list.normtime, 
	                                        order_head.order_num, 
	                                        order_head.order_name, 
	                                        common_ul_directory.ul_name, 
	                                        common_equip_directory.equip_name, 
	                                        man_planjob_list.id_norm_operation, 
	                                        man_idletime.idletime_type, 
	                                        man_idletime.id_idletime, 
	                                        idletime_directory.idletime_name, 
	                                        man_idletime.id_man_idletime, 
	                                        order_head.id_order_head,
                                            norm_operation_table.ord
                                        FROM
	                                        dbo.man_planjob
	                                        INNER JOIN
	                                        dbo.man_planjob_list
	                                        ON 
		                                        man_planjob.id_man_order_job_item = man_planjob_list.id_man_order_job_item
	                                        LEFT JOIN
	                                        dbo.man_order_job_item
	                                        ON 
		                                        man_planjob.id_man_order_job_item = man_order_job_item.id_man_order_job_item
	                                        LEFT JOIN
	                                        dbo.man_order_job
	                                        ON 
		                                        man_order_job_item.id_man_order_job = man_order_job.id_man_order_job
	                                        LEFT JOIN
	                                        dbo.order_head
	                                        ON 
		                                        man_order_job.id_order_head = order_head.id_order_head
	                                        LEFT JOIN
	                                        dbo.common_ul_directory
	                                        ON 
		                                        order_head.id_customer = common_ul_directory.id_common_ul_directory
	                                        LEFT JOIN
	                                        dbo.common_equip_directory
	                                        ON 
		                                        man_order_job.id_equip = common_equip_directory.id_common_equip_directory
	                                        LEFT JOIN
	                                        dbo.man_idletime
	                                        ON 
		                                        man_order_job.id_man_order_job = man_idletime.id_man_order_job
	                                        LEFT JOIN
	                                        dbo.idletime_directory
	                                        ON 
		                                        man_idletime.id_idletime = idletime_directory.id_idletime_directory
                                            LEFT JOIN
	                                        dbo.norm_operation_table
	                                        ON 
		                                        man_planjob_list.id_norm_operation = norm_operation_table.id_norm_operation
                                        WHERE
	                                        man_planjob.status <> 2 AND
	                                        man_planjob.flags <> 1 AND
                                            plan_out_qty IS NOT NULL AND
	                                        man_planjob.id_equip = @idMachine
                                        ORDER BY
	                                        man_planjob.date_begin ASC"
                    };
                    Command.Parameters.AddWithValue("@idMachine", idMachine);

                    DbDataReader sqlReader = Command.ExecuteReader();

                    while (sqlReader.Read())
                    {
                        if (token.IsCancellationRequested)
                        {
                            break;
                        }

                        string operationStatus = "";

                        if ((int)sqlReader["flags"] == 8 || (int)sqlReader["flags"] == 10 || (int)sqlReader["flags"] == 40)
                        {
                            operationStatus = "В плане";
                        }

                        if ((int)sqlReader["flags"] == 0 || (int)sqlReader["flags"] == 32)
                        {
                            operationStatus = "В очереди";
                        }

                        if ((int)sqlReader["status"] == 1 || (int)sqlReader["status"] == 2 || (int)sqlReader["status"] == 3)
                        {
                            operationStatus = "В работе";
                        }

                        int idManPlanJob = Convert.ToInt32(sqlReader["id_man_planjob"]);

                        if (!DBNull.Value.Equals(sqlReader["order_num"]))
                        {
                            //подумать над реализацией
                            int lastIutemIndex = orders.Count - 1;
                            int itemIndex = orders.FindIndex((v) => v.IDManPlanJob == idManPlanJob);

                            if (itemIndex == -1)
                            {
                                orders.Add(new OrdersLoad(
                                        0,
                                        idManPlanJob,
                                        sqlReader["date_begin"].ToString(),
                                        sqlReader["date_end"].ToString(),
                                        sqlReader["order_num"].ToString(),
                                        sqlReader["ul_name"].ToString(),
                                        sqlReader["order_name"].ToString(),
                                        0,
                                        0,
                                        0,
                                        operationStatus,
                                        sqlReader["id_order_head"].ToString()
                                    ));

                                itemIndex = orders.Count - 1;
                            }

                            if ((int)sqlReader["ord"] == 0)
                            {
                                orders[itemIndex].makereadyTime = sqlReader["normtime"] == DBNull.Value ? 0 : Convert.ToInt32(sqlReader["normtime"]) / Convert.ToInt32(sqlReader["plan_out_qty"]);
                            }

                            if ((int)sqlReader["ord"] == 1)
                            {
                                orders[itemIndex].workTime = sqlReader["normtime"] == DBNull.Value ? 0 : Convert.ToInt32(sqlReader["normtime"]);
                                orders[itemIndex].amountOfOrder = Convert.ToInt32(sqlReader["plan_out_qty"]);
                            }

                            if (orders[orders.Count - 1].IDManPlanJob != idManPlanJob)
                            {
                                //AddOrderToListView(itemIndex, orders[itemIndex], token);
                                lastItemIndex = itemIndex;
                            }
                        }
                        else
                        {
                            int itemIndex = orders.FindIndex((v) => v.IDManPlanJob == idManPlanJob);

                            if (itemIndex == -1)
                            {
                                orders.Add(new OrdersLoad(
                                        1,
                                        idManPlanJob,
                                        sqlReader["date_begin"].ToString(),
                                        sqlReader["date_end"].ToString(),
                                        "",
                                        "",
                                        sqlReader["idletime_name"].ToString(),
                                        0,
                                        sqlReader["normtime"] == DBNull.Value ? 0 : Convert.ToInt32(sqlReader["normtime"]), //Convert.ToInt32(sqlReader["normtime"]),
                                        0,
                                        operationStatus,
                                        ""
                                    ));

                                itemIndex = orders.Count - 1;
                            }

                            //AddOrderToListView(itemIndex, orders[itemIndex], token);
                        }

                        if (token.IsCancellationRequested)
                        {
                            break;
                        }
                    }

                    Connect.Close();

                    return orders;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                //Logger.WriteLine(ex.Message);
                return null;
            }
        }

/*        public async Task<List<string>> GetMachines(string userID)
        {
            List<string> result = new List<string>();
            //result.Clear();

            try
            {
                using (MySqlConnection Connect = DBConnection.GetDBConnection())
                {
                    await Connect.OpenAsync();
                    MySqlCommand Command = new MySqlCommand
                    {
                        Connection = Connect,
                        CommandText = @"SELECT * FROM machinesInfo WHERE nameOfExecutor = '" + userID + "'"
                    };
                    DbDataReader sqlReader = await Command.ExecuteReaderAsync();

                    while (await sqlReader.ReadAsync())
                    {
                        result.Add(sqlReader["machine"].ToString());
                        //result.Add(sqlReader["machine"] == DBNull.Value ? string.Empty : (string)sqlReader["machine"]);
                    }

                    await Connect.CloseAsync();
                }

                return result;
            }
            catch (SqlException sqlEx)
            {
                LogException.WriteLine("GetMachines: " + string.Format("MySQL #{0}: {1}", sqlEx.Number, sqlEx.Message));
                throw new ApplicationException(string.Format("MySQL #{0}: {1}", sqlEx.Number, sqlEx.Message));
            }
            catch (Exception ex)
            {
                LogException.WriteLine("GetMachines: " + ex.Message);
                throw new ApplicationException(ex.Message);
            }
        }

        public async Task<string> GetMachinesStr(string userID)
        {
            ValueInfoBase getInfo = new ValueInfoBase();

            List<string> orderMachines = await GetMachines(userID);
            string machines = "";

            for (int i = 0; i < orderMachines.Count; i++)
            {
                machines += await GetMachineName(orderMachines[i]);

                if (i != orderMachines.Count - 1)
                    machines += ", ";
                else
                    machines += ".";
            }

            return machines;
        }
*/


/*        public void DeleteMachine(String id)
        {
            using (MySqlConnection Connect = DBConnection.GetDBConnection())
            {
                string commandText = "DELETE FROM machines WHERE id = @id";

                MySqlCommand Command = new MySqlCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@id", id);
                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();
            }

            using (MySqlConnection Connect = DBConnection.GetDBConnection())
            {
                string commandText = "DELETE FROM machinesInfo WHERE machine = @id";

                MySqlCommand Command = new MySqlCommand(commandText, Connect);
                Command.Parameters.AddWithValue("@id", id);
                Connect.Open();
                Command.ExecuteNonQuery();
                Connect.Close();
            }
        }*/

    }
}
