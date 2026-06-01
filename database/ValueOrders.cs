using data;
using database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;

namespace Production_planning
{
    public class ValueOrders
    {
        public ValueOrders()
        {

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
