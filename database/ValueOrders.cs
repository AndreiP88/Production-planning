using Dapper;
using data;
using database;
using MySqlConnector;
using Mysqlx.Crud;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

        public async Task<List<ProductionJobCard>> GetProductionJobsForAllEquipsAsync(DateTime startDate, DateTime endDate, string externalConnString, int? targetNormOperationId = null)
        {
            // ИСПРАВЛЕНО: Убрана лишняя запятая перед FROM
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
          COALESCE(idletime_directory.idletime_name, '') AS IdleTimeName,
          order_head.id_order_head
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
          AND plan_out_qty IS NOT NULL
          AND man_planjob.date_begin <= @EndDate 
          AND man_planjob.date_end >= @StartDate
        ORDER BY man_planjob.date_begin ASC;";

            // 🌟 ИСПРАВЛЕНИЕ ТАЙМАУТА: Выделяем 4000 мс на чтение и вызовы процедур по сети
            using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(4000)))
            {
                try
                {
                    using (var conn = new SqlConnection(externalConnString))
                    {
                        await conn.OpenAsync(cts.Token);

                        // Качаем сырой плоский поток строк асинхронно через Dapper
                        var rawRows = await conn.QueryAsync(new CommandDefinition(
                            sql,
                            new { StartDate = startDate.Date, EndDate = endDate.Date },
                            cancellationToken: cts.Token
                        ));

                        if (rawRows == null || !rawRows.Any())
                        {
                            return new List<ProductionJobCard>();
                        }

                        // ИНТЕЛЛЕКТУАЛЬНАЯ СБОРКА И СКЛЕЙКА ФАЗ
                        var structuredJobs = rawRows
                            .GroupBy(r => r.Ord == null
                                ? $"IDLE_{r.IdPlanJob}_{r.DateBegin:yyyyMMdd}_{r.IdEquip}"
                                : $"JOB_{r.IdPlanJob}_{r.IdEquip}")
                            .Select(group =>
                            {
                                var baseRow = group.First();

                                var card = new ProductionJobCard
                                {
                                    IdPlanJob = baseRow.IdPlanJob != null ? (long)baseRow.IdPlanJob : 0,
                                    DateBegin = baseRow.DateBegin != null ? (DateTime)baseRow.DateBegin : DateTime.MinValue,
                                    DateEnd = baseRow.DateEnd != null ? (DateTime)baseRow.DateEnd : DateTime.MinValue,
                                    IdEquip = baseRow.IdEquip != null ? (int)baseRow.IdEquip : 0,
                                    OrderNum = baseRow.OrderNum != null ? (string)baseRow.OrderNum : string.Empty,
                                    OrderName = baseRow.OrderName != null ? (string)baseRow.OrderName : string.Empty,
                                    CustomerName = baseRow.UlName != null ? (string)baseRow.UlName : string.Empty,
                                    IsIdleTime = baseRow.Ord == null,
                                    IdleTimeName = baseRow.IdleTimeName != null ? (string)baseRow.IdleTimeName : string.Empty,

                                    // Сохраняем связи для оснастки
                                    IdOrderHead = baseRow.id_order_head != null ? (int)baseRow.id_order_head : 0,

                                    // Виртуальный маппинг операции: берем внешний ID (если передан для склейки), иначе - родной
                                    IdNormOperation = (targetNormOperationId.HasValue && targetNormOperationId.Value > 0)
                                        ? targetNormOperationId.Value
                                        : (baseRow.IdNormOperation != null ? (int)baseRow.IdNormOperation : 0),

                                    FullToolName = "—",
                                    SetupCount = 0,
                                    SetupNormTime = 0,
                                    RunNormTime = 0,
                                    RunQty = 0
                                };

                                if (!card.IsIdleTime)
                                {
                                    var setupRow = group.FirstOrDefault(r => r.Ord == 0);
                                    if (setupRow != null)
                                    {
                                        card.SetupCount = setupRow.PlanOutQty != null ? (double)setupRow.PlanOutQty : 0;
                                        card.SetupNormTime = card.SetupCount > 0 && setupRow.NormTime != null
                                            ? (double)setupRow.NormTime / card.SetupCount
                                            : 0;
                                    }

                                    var runRow = group.FirstOrDefault(r => r.Ord == 1);
                                    if (runRow != null)
                                    {
                                        card.RunQty = runRow.PlanOutQty != null ? (double)runRow.PlanOutQty : 0;
                                        card.RunNormTime = runRow.NormTime != null ? (double)runRow.NormTime : 0;
                                    }
                                }
                                else
                                {
                                    card.RunNormTime = baseRow.NormTime != null ? (double)baseRow.NormTime : 0;
                                }

                                return card;
                            })
                            .ToList();

                        // -----------------------------------------------------------------
                        // СБОРКА КЭША ОСНАСТКИ ЧЕРЕЗ ЗАКРЫТУЮ ПРОЦЕДУРУ
                        // -----------------------------------------------------------------
                        var uniqueOrderIds = structuredJobs
                            .Where(j => j.IdOrderHead > 0 && !j.IsIdleTime)
                            .Select(j => j.IdOrderHead)
                            .Distinct()
                            .ToList();

                        var toolCache = new Dictionary<string, string>();

                        foreach (var orderId in uniqueOrderIds)
                        {
                            try
                            {
                                // Вызываем процедуру через Dapper (он автоматически управляет открытием и закрытием Reader)
                                var procRows = await conn.QueryAsync(
                                    "exec proc_rpt_order_tool @id_order_head, 1, 16, 1",
                                    new { id_order_head = orderId },
                                    commandTimeout: 2 // Лимит 2 секунды на одну процедуру
                                );

                                if (procRows != null)
                                {
                                    foreach (var row in procRows)
                                    {
                                        // Приводим динамический результат Dapper к словарю для безопасного чтения колонок
                                        var dictRow = row as IDictionary<string, object>;
                                        if (dictRow != null && dictRow.ContainsKey("id_norm_operation") && dictRow.ContainsKey("tool_name"))
                                        {
                                            var opObj = dictRow["id_norm_operation"];
                                            var toolObj = dictRow["tool_name"];

                                            if (opObj != null && toolObj != null)
                                            {
                                                int normOpId = Convert.ToInt32(opObj);
                                                string toolName = toolObj.ToString();

                                                string cacheKey = $"{orderId}_{normOpId}";
                                                toolCache[cacheKey] = toolName;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception procEx)
                            {
                                // Если по какому-то одному заказу процедура легла — не ломаем весь план, просто пишем лог
                                Console.WriteLine($"Ошибка proc_rpt_order_tool для заказа {orderId}: {procEx.Message}");
                            }
                        }

                        // Маппим штампы на наши собранные карточки планов
                        foreach (var job in structuredJobs)
                        {
                            if (job.IsIdleTime) continue;

                            string searchKey = $"{job.IdOrderHead}_{job.IdNormOperation}";
                            if (toolCache.ContainsKey(searchKey))
                            { job.FullToolName = toolCache[searchKey]; }
                        }
                        return structuredJobs;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: GetProductionJobsAsync " + ex.Message);// Чтобы при ошибке сеть не возвращала null и не вешала форму, возвращаем пустой инициализированный список

                    return null;
                }
            }
        }

        public async Task<List<ProductionJobCard>> GetProductionJobsAsync(DateTime startDate, DateTime endDate, string externalConnString, int externalEquipId)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8)))
            {
                try
                {
                    using (var conn = new SqlConnection(externalConnString))
                    {
                        await conn.OpenAsync(cts.Token);

                        // -----------------------------------------------------------------
                        // ШАГ 1: Получаем базовый план-наряд (Оригинальный рабочий регистр полей)
                        // -----------------------------------------------------------------
                        var planParams = new { dtDateBeg = startDate, dtDateEnd = endDate, nObjectID = externalEquipId, nObjectType = 10, nReportId = 611 };
                        var rawPlan = await conn.QueryAsync("exec proc_rpt_job_plan @dtDateBeg, @dtDateEnd, @nObjectID, @nObjectType, @nReportId", planParams, commandTimeout: 5);

                        if (rawPlan == null || !rawPlan.Any()) return new List<ProductionJobCard>();

                        // ГРУППИРУЕМ ПРИЛАДКУ И РАБОТУ В ОДНУ СТРОКУ ЗАКАЗА
                        var structuredJobs = rawPlan
                            .GroupBy(r => r.ord == null || string.IsNullOrEmpty(r.order_num?.ToString())
                                ? $"IDLE_{r.id_man_planjob}_{r.date_begin:yyyyMMdd}"
                                : $"JOB_{r.id_man_planjob}")
                            .Select(group =>
                            {
                                var baseRow = group.First();
                                bool isIdle = string.IsNullOrEmpty(baseRow.order_num?.ToString()) || baseRow.ord == null;

                                var card = new ProductionJobCard
                                {
                                    IdPlanJob = baseRow.id_man_planjob != null ? Convert.ToInt64(baseRow.id_man_planjob) : 0,
                                    IdEquip = baseRow.id_common_equip_directory != null ? Convert.ToInt32(baseRow.id_common_equip_directory) : -1,
                                    OrderNum = baseRow.order_num?.ToString() ?? string.Empty,
                                    CustomerName = baseRow.customer_name?.ToString() ?? string.Empty,
                                    OrderName = baseRow.order_name?.ToString() ?? string.Empty,
                                    DateBegin = group.Min(r => r.date_begin != null ? Convert.ToDateTime(r.date_begin) : DateTime.MaxValue),
                                    DateEnd = group.Max(r => r.date_end != null ? Convert.ToDateTime(r.date_end) : DateTime.MinValue),
                                    IdOrderHead = baseRow.id_order_head != null ? Convert.ToInt32(baseRow.id_order_head) : 0,
                                    IsIdleTime = isIdle,
                                    IdleTimeName = isIdle ? (baseRow.operation_name?.ToString() ?? "Технологический простой") : string.Empty
                                };

                                if (!card.IsIdleTime)
                                {
                                    var setupRow = group.FirstOrDefault(r => Convert.ToInt32(r.ord) == 0);
                                    if (setupRow != null)
                                    {
                                        card.SetupCount = setupRow.plan_out_qty != null ? Convert.ToDouble(setupRow.plan_out_qty) : 0;
                                        card.SetupNormTime = setupRow.normtime != null ? Convert.ToDouble(setupRow.normtime) : 0;
                                    }

                                    var runRow = group.FirstOrDefault(r => Convert.ToInt32(r.ord) == 1);
                                    if (runRow != null)
                                    {
                                        card.RunQty = runRow.plan_out_qty != null ? Convert.ToDouble(runRow.plan_out_qty) : 0;
                                        card.RunNormTime = runRow.normtime != null ? Convert.ToDouble(runRow.normtime) : 0;
                                    }

                                    // СУММИРУЕМ ВРЕМЯ ФАЗ В ОДНУ ОБЩУЮ СТРОКУ ЗАКАЗА
                                    card.NormTimeMinutes = card.SetupNormTime + card.RunNormTime;
                                }
                                else
                                {
                                    card.NormTimeMinutes = baseRow.normtime != null ? Convert.ToDouble(baseRow.normtime) : 0;
                                }

                                return card;
                            }).ToList();

                        // -----------------------------------------------------------------
                        // ШАГ 2: Подтягиваем операции (Оригинальный рабочий регистр полей)
                        // -----------------------------------------------------------------
                        var operParams = new { dtDateBeg = startDate, dtDateEnd = endDate, nObjectID = externalEquipId, nObjectType = 10 };
                        var rawOpers = await conn.QueryAsync("exec proc_rpt_job_plan_oper_list @dtDateBeg, @dtDateEnd, @nObjectID, @nObjectType", operParams);

                        if (rawOpers != null)
                        {
                            var operCache = new Dictionary<long, dynamic>();
                            foreach (var op in rawOpers)
                            {
                                if (op.id_man_planjob != null && Convert.ToInt32(op.main_suboper) == 1)
                                    operCache[Convert.ToInt64(op.id_man_planjob)] = op;

                                if (op.id_man_planjob != null && Convert.ToInt32(op.main_suboper) == 0)
                                    operCache[Convert.ToInt64(op.id_man_planjob)] = op;
                            }

                            foreach (var job in structuredJobs)
                            {
                                if (operCache.ContainsKey(job.IdPlanJob))
                                {
                                    var matchedOp = operCache[job.IdPlanJob];
                                    job.ItemName = matchedOp.itemname?.ToString() ?? string.Empty;
                                    job.OperProp = matchedOp.oper_prop?.ToString() ?? string.Empty;
                                    job.NoteManagerTech = matchedOp.note_manager_tech?.ToString() ?? string.Empty;

                                    // Вытаскиваем itemid для последующего точечного запроса красок
                                    if (matchedOp.itemid != null) job.ItemId = Convert.ToInt64(matchedOp.itemid);
                                    else if (matchedOp.item_id != null) job.ItemId = Convert.ToInt64(matchedOp.item_id);

                                    if (job.IsIdleTime)
                                    {
                                        job.NormTimeMinutes = Convert.ToInt32(matchedOp.normtime);
                                    }
                                }
                            }
                        }

                        // Извлекаем уникальные ID для связи штампов и бумаги
                        var uniqueOrderIds = structuredJobs.Where(j => j.IdOrderHead > 0 && !j.IsIdleTime).Select(j => j.IdOrderHead).Distinct().ToList();

                        // -----------------------------------------------------------------
                        // 🌟 ШАГ 2.5: ТОЧЕЧНАЯ КОРРЕКТИРОВКА ОТОБРАЖЕНИЯ КРАСОК (ЛИЦО И ОБОРОТ) 🌟
                        // -----------------------------------------------------------------
                        string techOpSql = @"
                    SELECT TOP 1 order_job.id_order_technology_op
                    FROM dbo.man_order_job
                    INNER JOIN dbo.man_order_job_item ON man_order_job.id_man_order_job = man_order_job_item.id_man_order_job
                    INNER JOIN dbo.order_job ON man_order_job.id_order_job = order_job.id_order_job
                    INNER JOIN dbo.man_planjob ON man_order_job_item.id_man_order_job_item = man_planjob.id_man_order_job_item 
                    WHERE man_order_job_item.itemid = @ItemId AND man_planjob.id_man_planjob = @IdPlanJob";

                        foreach (var job in structuredJobs)
                        {
                            if (job.IsIdleTime || job.ItemId <= 0) continue;

                            try
                            {
                                var techOpRow = await conn.QueryFirstOrDefaultAsync(techOpSql, new { ItemId = job.ItemId, IdPlanJob = job.IdPlanJob });
                                if (techOpRow != null && techOpRow.id_order_technology_op != null)
                                {
                                    long techOpId = Convert.ToInt64(techOpRow.id_order_technology_op);

                                    var paints = await conn.QueryAsync(
                                        "exec proc_rpt_order_tmap_oper_paint_new @id_order_head, @id_order_technology_op, 1",
                                        new { id_order_head = job.IdOrderHead, id_order_technology_op = techOpId }
                                    );

                                    if (paints != null && paints.Any())
                                    {
                                        var pRow = paints.First();
                                        string face = pRow.color_list_face?.ToString()?.Trim() ?? string.Empty;
                                        string back = pRow.color_list_back?.ToString()?.Trim() ?? string.Empty;

                                        bool hasFacePaint = !string.IsNullOrEmpty(face) && face != "—" && face != "0";
                                        bool hasBackPaint = !string.IsNullOrEmpty(back) && back != "—" && back != "0" && back != "0+0";

                                        // 🌟 Условие: Буквы Л и О выводятся, только если заполнена конкретная сторона
                                        if (hasFacePaint && hasBackPaint)
                                        {
                                            job.OperProp = $"Л: {face}\r\nО: {back}";
                                        }
                                        else if (hasFacePaint)
                                        {
                                            job.OperProp = face; // Прячем "Л:", если оборота нет
                                        }
                                        else if (hasBackPaint)
                                        {
                                            job.OperProp = $"О: {back}"; // Прячем "Л:", если есть только оборот
                                        }
                                        else
                                        {
                                            job.OperProp = "—";
                                        }
                                    }
                                }
                            }
                            catch (Exception paintEx)
                            {
                                Console.WriteLine($"Ошибка получения красок: {paintEx.Message}");
                            }
                        }

                        // ШАГ 3: Оснастка
                        var toolCache = new Dictionary<int, string>();
                        foreach (var orderId in uniqueOrderIds)
                        {
                            var tools = await conn.QueryAsync("exec proc_rpt_order_tool @id_order_head, 1, 16, 1", new { id_order_head = orderId });
                            if (tools != null)
                            {
                                var uniqueTools = tools.Where(t => t.id_norm_operation != null && Convert.ToInt32(t.id_norm_operation) == 12)
                                .Where(t => t.tool_name != null).Select(t => t.tool_name.ToString()).Distinct().ToList();
                                if (uniqueTools.Any()) toolCache[orderId] = string.Join(", ", uniqueTools);
                            }
                        }
                        foreach (var job in structuredJobs)
                        {
                            if (toolCache.ContainsKey(job.IdOrderHead)) job.FullToolName = toolCache[job.IdOrderHead];
                        }
                        // ШАГ 4: Материал
                        var paperCache = new Dictionary<string, dynamic>();
                        foreach (var orderId in uniqueOrderIds)
                        {
                            var papers = await conn.QueryAsync("exec proc_rpt_order_paper_BY @id_order_head, 16, 1, 0, 1, 611, 16", new { id_order_head = orderId });
                            if (papers != null)
                            {
                                foreach (var p in papers)
                                {
                                    if (p.sheet_name != null) paperCache[orderId.ToString()] = p;
                                }
                            }
                        }
                        foreach (var job in structuredJobs)
                        {
                            if (job.IsIdleTime) continue;
                            string searchKey = job.IdOrderHead.ToString();
                            if (paperCache.ContainsKey(searchKey))
                            {
                                var matchedPaper = paperCache[searchKey];
                                job.PaperName = matchedPaper.paper_name?.ToString() ?? string.Empty;
                                job.FormatPaper = matchedPaper.format_paper?.ToString() ?? string.Empty;
                                job.FormatCutting = matchedPaper.format_cutting?.ToString() ?? string.Empty;
                                job.Density = matchedPaper.density != null ? Convert.ToDouble(matchedPaper.density) : 0;
                            }
                        }
                        return structuredJobs;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CRITICAL ERROR: GetProductionJobsAsync -> " + ex.Message);
                    return new List<ProductionJobCard>();
                }
            }
        }






        /*public async Task<List<ProductionJobCard>> GetProductionJobsAsync(DateTime startDate, DateTime endDate, string externalConnString, int externalEquipId)
        {
            // Увеличили лимит до 10 секунд из-за каскадных вызовов процедур красочности по сети
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                try
                {
                    using (var conn = new SqlConnection(externalConnString))
                    {
                        await conn.OpenAsync(cts.Token);

                        // -----------------------------------------------------------------
                        // ШАГ 1: Получаем базовый план-наряд (proc_rpt_job_plan)
                        // -----------------------------------------------------------------
                        var planParams = new { dtDateBeg = startDate, dtDateEnd = endDate, nObjectID = externalEquipId, nObjectType = 10, nReportId = 611 };
                        var rawPlan = await conn.QueryAsync("exec proc_rpt_job_plan @dtDateBeg, @dtDateEnd, @nObjectID, @nObjectType, @nReportId", planParams, commandTimeout: 5);

                        if (rawPlan == null || !rawPlan.Any()) return new List<ProductionJobCard>();

                        // ГРУППИРУЕМ ПРИЛАДКУ И РАБОТУ В ОДНУ СТРОКУ ЗАКАЗА
                        var structuredJobs = rawPlan
                            .GroupBy(r => r.ord == null || string.IsNullOrEmpty(r.order_num?.ToString())
                                ? $"IDLE_{r.id_man_planjob}_{r.date_begin:yyyyMMdd}"
                                : $"JOB_{r.id_man_planjob}")
                            .Select(group =>
                            {
                                var baseRow = group.First();
                                bool isIdle = string.IsNullOrEmpty(baseRow.order_num?.ToString()) || baseRow.ord == null;

                                var card = new ProductionJobCard
                                {
                                    IdPlanJob = baseRow.id_man_planjob != null ? Convert.ToInt64(baseRow.id_man_planjob) : 0,
                                    IdEquip = baseRow.id_common_equip_directory != null ? Convert.ToInt32(baseRow.id_common_equip_directory) : -1,
                                    OrderNum = baseRow.order_num?.ToString() ?? string.Empty,
                                    CustomerName = baseRow.customer_name?.ToString() ?? string.Empty,
                                    OrderName = baseRow.order_name?.ToString() ?? string.Empty,
                                    DateBegin = group.Min(r => r.date_begin != null ? Convert.ToDateTime(r.date_begin) : DateTime.MaxValue),
                                    DateEnd = group.Max(r => r.date_end != null ? Convert.ToDateTime(r.date_end) : DateTime.MinValue),
                                    IdOrderHead = baseRow.id_order_head != null ? Convert.ToInt32(baseRow.id_order_head) : 0,
                                    IsIdleTime = isIdle,
                                    IdleTimeName = isIdle ? (baseRow.operation_name?.ToString() ?? "Технологический простой") : string.Empty
                                };

                                if (!card.IsIdleTime)
                                {
                                    var setupRow = group.FirstOrDefault(r => Convert.ToInt32(r.ord) == 0);
                                    if (setupRow != null)
                                    {
                                        card.SetupCount = setupRow.plan_out_qty != null ? Convert.ToDouble(setupRow.plan_out_qty) : 0;
                                        card.SetupNormTime = setupRow.normtime != null ? Convert.ToDouble(setupRow.normtime) : 0;
                                    }

                                    var runRow = group.FirstOrDefault(r => Convert.ToInt32(r.ord) == 1);
                                    if (runRow != null)
                                    {
                                        card.RunQty = runRow.plan_out_qty != null ? Convert.ToDouble(runRow.plan_out_qty) : 0;
                                        card.RunNormTime = runRow.normtime != null ? Convert.ToDouble(runRow.normtime) : 0;
                                    }

                                    // СУММИРУЕМ ВРЕМЯ ФАЗ В ОДНУ ОБЩУЮ СТРОКУ ЗАКАЗА
                                    card.NormTimeMinutes = card.SetupNormTime + card.RunNormTime;
                                }
                                else
                                {
                                    card.NormTimeMinutes = baseRow.normtime != null ? Convert.ToDouble(baseRow.normtime) : 0;
                                }

                                return card;
                            }).ToList();

                        // -----------------------------------------------------------------
                        // ШАГ 2: Подтягиваем операции (proc_rpt_job_plan_oper_list)
                        // -----------------------------------------------------------------
                        var operParams = new { dtDateBeg = startDate, dtDateEnd = endDate, nObjectID = externalEquipId, nObjectType = 10 };
                        var rawOpers = await conn.QueryAsync("exec proc_rpt_job_plan_oper_list @dtDateBeg, @dtDateEnd, @nObjectID, @nObjectType", operParams);

                        if (rawOpers != null)
                        {
                            // Кэш для рабочих операций (main_suboper = 1)
                            var operCache = new Dictionary<long, dynamic>();
                            // Кэш для технологических простоев
                            var idleOperCache = new Dictionary<long, dynamic>();

                            foreach (var op in rawOpers)
                            {
                                if (op.id_man_planjob != null)
                                {
                                    long planJobId = Convert.ToInt64(op.id_man_planjob);
                                    if (op.main_suboper != null && Convert.ToInt32(op.main_suboper) == 1)
                                    {
                                        operCache[planJobId] = op;
                                    }
                                    else if (op.main_suboper == null)
                                    {
                                        idleOperCache[planJobId] = op;
                                    }
                                }
                            }

                            foreach (var job in structuredJobs)
                            {
                                if (job.IsIdleTime)
                                {
                                    if (idleOperCache.ContainsKey(job.IdPlanJob))
                                    {
                                        var matchedIdle = idleOperCache[job.IdPlanJob];
                                        job.ItemName = matchedIdle.itemname?.ToString() ?? string.Empty;
                                        job.NoteManagerTech = matchedIdle.note_manager_tech?.ToString() ?? string.Empty;

                                        // 🌟 1. ДЛИТЕЛЬНОСТЬ ПРОСТОЯ: Берем из normtime_total
                                        if (matchedIdle.normtime != null)
                                        {
                                            job.NormTimeMinutes = Convert.ToDouble(matchedIdle.normtime);
                                        }
                                    }
                                }
                                else
                                {
                                    if (operCache.ContainsKey(job.IdPlanJob))
                                    {
                                        var matchedOp = operCache[job.IdPlanJob];
                                        job.ItemName = matchedOp.itemname?.ToString() ?? string.Empty;
                                        job.OperProp = matchedOp.oper_prop?.ToString() ?? string.Empty;
                                        job.NoteManagerTech = matchedOp.note_manager_tech?.ToString() ?? string.Empty;

                                        // Запоминаем itemid из операции для последующего поиска красок
                                        if (matchedOp.itemid != null) job.ItemId = Convert.ToInt64(matchedOp.itemid);
                                        else if (matchedOp.item_id != null) job.ItemId = Convert.ToInt64(matchedOp.item_id);
                                    }
                                }
                            }
                        }

                        // -----------------------------------------------------------------
                        // ШАГ 2.5: ЗАПРОС КРАСОК (Лицо + Оборот) ПО ТЕХНОЛОГИЧЕСКОЙ ЦЕПОЧКЕ
                        // -----------------------------------------------------------------
                        string techOpSql = @"
                    SELECT TOP 1 order_job.id_order_technology_op
                    FROM dbo.man_order_job
                    INNER JOIN dbo.man_order_job_item ON man_order_job.id_man_order_job = man_order_job_item.id_man_order_job
                    INNER JOIN dbo.order_job ON man_order_job.id_order_job = order_job.id_order_job
                    INNER JOIN dbo.man_planjob ON man_order_job_item.id_man_order_job_item = man_planjob.id_man_order_job_item 
                    WHERE man_order_job_item.itemid = @ItemId AND man_planjob.id_man_planjob = @IdPlanJob";

                        foreach (var job in structuredJobs)
                        {
                            if (job.IsIdleTime || job.ItemId <= 0) continue;

                            try
                            {
                                // Находим id_order_technology_op
                                var techOpRow = await conn.QueryFirstOrDefaultAsync(techOpSql, new { ItemId = job.ItemId, IdPlanJob = job.IdPlanJob });
                                if (techOpRow != null && techOpRow.id_order_technology_op != null)
                                {
                                    long techOpId = Convert.ToInt64(techOpRow.id_order_technology_op);

                                    // Получаем списки красок из закрытой процедуры красочности
                                    var paints = await conn.QueryAsync(
                                        "exec proc_rpt_order_tmap_oper_paint_new @id_order_head, @id_order_technology_op, 1",
                                        new { id_order_head = job.IdOrderHead, id_order_technology_op = techOpId }
                                    );

                                    if (paints != null && paints.Any())
                                    {
                                        var pRow = paints.First();
                                        string face = pRow.color_list_face?.ToString() ?? "—";
                                        string back = pRow.color_list_back?.ToString() ?? "—";
                                        // 🌟 2. ДВУХСТРОЧНЫЙ ВЫВОД КРАСОК С ПЕРЕНОСОМ СТРОКИ
                                        job.OperProp = $"Л: {face}\r\nО: {back}";
                                    }
                                }
                            }
                            catch (Exception paintEx)
                            {
                                Console.WriteLine($"Ошибка proc_rpt_order_tmap_oper_paint_new для {job.IdPlanJob}: {paintEx.Message}");
                            }
                        }
                        // Извлекаем уникальные ID заголовков для штампов и материалов
                        var uniqueOrderIds = structuredJobs.Where(j => j.IdOrderHead > 0 && !j.IsIdleTime).Select(j => j.IdOrderHead).Distinct().ToList();
                        // ШАГ 3: Оснастка (proc_rpt_order_tool)
                        var toolCache = new Dictionary<int, string>();
                        foreach (var orderId in uniqueOrderIds)
                        {
                            var tools = await conn.QueryAsync("exec proc_rpt_order_tool @id_order_head, 1, 16, 1", new { id_order_head = orderId });
                            if (tools != null)
                            {
                                var uniqueTools = tools.Where(t => t.id_norm_operation != null && Convert.ToInt32(t.id_norm_operation) == 12)
                                .Where(t => t.tool_name != null).Select(t => t.tool_name.ToString()).Distinct().ToList();
                                if (uniqueTools.Any()) toolCache[orderId] = string.Join(", ", uniqueTools);
                            }
                        }
                        foreach (var job in structuredJobs)
                        {
                            if (toolCache.ContainsKey(job.IdOrderHead)) job.FullToolName = toolCache[job.IdOrderHead];
                        }
                        // ШАГ 4: Материал (proc_rpt_order_paper_BY)
                        var paperCache = new Dictionary<string, dynamic>();
                        foreach (var orderId in uniqueOrderIds)
                        {
                            var papers = await conn.QueryAsync("exec proc_rpt_order_paper_BY @id_order_head, 16, 1, 0, 1, 611, 16", new { id_order_head = orderId });
                            if (papers != null)
                            {
                                foreach (var p in papers)
                                {
                                    if (p.sheet_name != null) paperCache[orderId.ToString()] = p;
                                }
                            }
                        }
                        foreach (var job in structuredJobs)
                        {
                            if (job.IsIdleTime) continue;
                            string searchKey = job.IdOrderHead.ToString();
                            if (paperCache.ContainsKey(searchKey))
                            {
                                var matchedPaper = paperCache[searchKey];
                                job.PaperName = matchedPaper.paper_name?.ToString() ?? string.Empty;
                                job.FormatPaper = matchedPaper.format_paper?.ToString() ?? string.Empty;
                                job.FormatCutting = matchedPaper.format_cutting?.ToString() ?? string.Empty;
                                job.Density = matchedPaper.density != null ? Convert.ToDouble(matchedPaper.density) : 0;
                            }
                        }
                        return structuredJobs;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CRITICAL ERROR: GetProductionJobsAsync -> " + ex.Message);
                    return new List<ProductionJobCard>();
                }
            }
        }*/
        /*public async Task<List<ProductionJobCard>> GetProductionJobsAsync(DateTime startDate, DateTime endDate, string externalConnString, int externalEquipId)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8)))
            {
                try
                {
                    using (var conn = new SqlConnection(externalConnString))
                    {
                        await conn.OpenAsync(cts.Token);

                        var planParams = new { dtDateBeg = startDate, dtDateEnd = endDate, nObjectID = externalEquipId, nObjectType = 10, nReportId = 611 };
                        var rawPlan = await conn.QueryAsync("exec proc_rpt_job_plan @dtDateBeg, @dtDateEnd, @nObjectID, @nObjectType, @nReportId", planParams, commandTimeout: 5);

                        if (rawPlan == null || !rawPlan.Any()) return new List<ProductionJobCard>();

                        // 🌟 ГРУППИРУЕМ ПРИЛАДКУ И РАБОТУ В ОДНУ СТРОКУ ЗАКАЗА
                        var structuredJobs = rawPlan
                            .GroupBy(r => r.ord == null || string.IsNullOrEmpty(r.order_num?.ToString())
                                ? $"IDLE_{r.id_man_planjob}_{r.date_begin:yyyyMMdd}"
                                : $"JOB_{r.id_man_planjob}")
                            .Select(group =>
                            {
                                var baseRow = group.First();
                                bool isIdle = string.IsNullOrEmpty(baseRow.order_num?.ToString()) || baseRow.ord == null;

                                var card = new ProductionJobCard
                                {
                                    IdPlanJob = baseRow.id_man_planjob != null ? Convert.ToInt64(baseRow.id_man_planjob) : 0,
                                    IdEquip = baseRow.id_common_equip_directory != null ? Convert.ToInt32(baseRow.id_common_equip_directory) : -1,
                                    OrderNum = baseRow.order_num?.ToString() ?? string.Empty,
                                    CustomerName = baseRow.customer_name?.ToString() ?? string.Empty,
                                    OrderName = baseRow.order_name?.ToString() ?? string.Empty,
                                    DateBegin = group.Min(r => r.date_begin != null ? Convert.ToDateTime(r.date_begin) : DateTime.MaxValue),
                                    DateEnd = group.Max(r => r.date_end != null ? Convert.ToDateTime(r.date_end) : DateTime.MinValue),
                                    IdOrderHead = baseRow.id_order_head != null ? Convert.ToInt32(baseRow.id_order_head) : 0,
                                    IsIdleTime = isIdle,
                                    IdleTimeName = isIdle ? (baseRow.operation_name?.ToString() ?? "Технологический простой") : string.Empty
                                };

                                if (!card.IsIdleTime)
                                {
                                    var setupRow = group.FirstOrDefault(r => Convert.ToInt32(r.ord) == 0);
                                    if (setupRow != null)
                                    {
                                        card.SetupCount = setupRow.plan_out_qty != null ? Convert.ToDouble(setupRow.plan_out_qty) : 0;
                                        card.SetupNormTime = setupRow.normtime != null ? Convert.ToDouble(setupRow.normtime) : 0;
                                    }

                                    var runRow = group.FirstOrDefault(r => Convert.ToInt32(r.ord) == 1);
                                    if (runRow != null)
                                    {
                                        card.RunQty = runRow.plan_out_qty != null ? Convert.ToDouble(runRow.plan_out_qty) : 0;
                                        card.RunNormTime = runRow.normtime != null ? Convert.ToDouble(runRow.normtime) : 0;
                                    }

                                    // 🌟 СУММИРУЕМ ВРЕМЯ ФАЗ В ОДНУ ОБЩУЮ СТРОКУ ЗАКАЗА
                                    card.NormTimeMinutes = card.SetupNormTime + card.RunNormTime;
                                }
                                else
                                {
                                    card.NormTimeMinutes = baseRow.normtime != null ? Convert.ToDouble(baseRow.normtime) : 0;
                                }

                                return card;
                            }).ToList();

                        // Шаг 2: Подтягиваем операции (используем main_suboper = 1 для данных выполнения)
                        var operParams = new { dtDateBeg = startDate, dtDateEnd = endDate, nObjectID = externalEquipId, nObjectType = 10 };
                        var rawOpers = await conn.QueryAsync("exec proc_rpt_job_plan_oper_list @dtDateBeg, @dtDateEnd, @nObjectID, @nObjectType", operParams);

                        if (rawOpers != null)
                        {
                            var operCache = new Dictionary<long, dynamic>();
                            foreach (var op in rawOpers)
                            {
                                if (op.id_man_planjob != null && Convert.ToInt32(op.main_suboper) == 1)
                                    operCache[Convert.ToInt64(op.id_man_planjob)] = op;


                            }

                            foreach (var job in structuredJobs)
                            {
                                if (operCache.ContainsKey(job.IdPlanJob))
                                {
                                    var matchedOp = operCache[job.IdPlanJob];
                                    job.ItemName = matchedOp.itemname?.ToString() ?? string.Empty;
                                    job.OperProp = matchedOp.oper_prop?.ToString() ?? string.Empty;
                                    job.NoteManagerTech = matchedOp.note_manager_tech?.ToString() ?? string.Empty;
                                }
                            }
                        }

                        var uniqueOrderIds = structuredJobs.Where(j => j.IdOrderHead > 0 && !j.IsIdleTime).Select(j => j.IdOrderHead).Distinct().ToList();

                        // Шаг 3: Оснастка
                        var toolCache = new Dictionary<int, string>();
                        foreach (var orderId in uniqueOrderIds)
                        {
                            var tools = await conn.QueryAsync("exec proc_rpt_order_tool @id_order_head, 1, 16, 1", new { id_order_head = orderId });
                            if (tools != null)
                            {
                                var uniqueTools = tools.Where(t => t.id_norm_operation != null && Convert.ToInt32(t.id_norm_operation) == 12)
                                                       .Where(t => t.tool_name != null).Select(t => t.tool_name.ToString()).Distinct().ToList();
                                if (uniqueTools.Any()) toolCache[orderId] = string.Join(", ", uniqueTools);
                            }
                        }
                        foreach (var job in structuredJobs)
                        {
                            if (toolCache.ContainsKey(job.IdOrderHead)) job.FullToolName = toolCache[job.IdOrderHead];
                        }

                        // Шаг 4: Материал
                        var paperCache = new Dictionary<string, dynamic>();
                        foreach (var orderId in uniqueOrderIds)
                        {
                            var papers = await conn.QueryAsync("exec proc_rpt_order_paper_BY @id_order_head, 16, 1, 0, 1, 611, 16", new { id_order_head = orderId });
                            if (papers != null)
                            {
                                foreach (var p in papers)
                                {
                                    if (p.sheet_name != null) paperCache[orderId.ToString()] = p;
                                }
                            }
                        }

                        foreach (var job in structuredJobs)
                        {
                            if (job.IsIdleTime) continue;
                            string searchKey = job.IdOrderHead.ToString();
                            if (paperCache.ContainsKey(searchKey))
                            {
                                var matchedPaper = paperCache[searchKey];
                                job.PaperName = matchedPaper.paper_name?.ToString() ?? string.Empty;
                                job.FormatPaper = matchedPaper.format_paper?.ToString() ?? string.Empty;
                                job.FormatCutting = matchedPaper.format_cutting?.ToString() ?? string.Empty;
                                job.Density = matchedPaper.density != null ? Convert.ToDouble(matchedPaper.density) : 0;
                            }
                        }

                        return structuredJobs;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CRITICAL ERROR: GetProductionJobsAsync -> " + ex.Message);
                    return new List<ProductionJobCard>();
                }
            }
        }*/


        /*public async Task<List<ProductionJobCard>> GetProductionJobsAsync(DateTime startDate, DateTime endDate, string externalConnString, int externalEquipId)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(8)))
            {
                try
                {
                    using (var conn = new SqlConnection(externalConnString))
                    {
                        await conn.OpenAsync(cts.Token);

                        // -----------------------------------------------------------------
                        // ШАГ 1: Получаем базовый план-наряд (Каждая строка — отдельная фаза)
                        // -----------------------------------------------------------------
                        var planParams = new
                        {
                            dtDateBeg = startDate,
                            dtDateEnd = endDate,
                            nObjectID = externalEquipId,
                            nObjectType = 10,
                            nReportId = 611
                        };

                        var rawPlan = await conn.QueryAsync(
                            "exec proc_rpt_job_plan @dtDateBeg, @dtDateEnd, @nObjectID, @nObjectType, @nReportId",
                            planParams,
                            commandTimeout: 5
                        );

                        if (rawPlan == null || !rawPlan.Any()) return new List<ProductionJobCard>();

                        // НЕ ГРУППИРУЕМ СТРОКИ! Оставляем приладку (Ord=0) и выполнение (Ord=1) раздельными строками, как в БД
                        var jobsList = rawPlan.Select(r => {
                            var card = new ProductionJobCard
                            {
                                IdPlanJob = r.id_man_planjob != null ? Convert.ToInt64(r.id_man_planjob) : 0,
                                IdEquip = r.id_common_equip_directory != null ? Convert.ToInt32(r.id_common_equip_directory) : -1,
                                OrderNum = r.order_num?.ToString() ?? string.Empty,
                                CustomerName = r.customer_name?.ToString() ?? string.Empty,
                                OrderName = r.order_name?.ToString() ?? string.Empty,
                                Ord = r.ord != null ? Convert.ToInt32(r.ord) : -1,
                                DateBegin = r.date_begin != null ? Convert.ToDateTime(r.date_begin) : DateTime.MinValue,
                                DateEnd = r.date_end != null ? Convert.ToDateTime(r.date_end) : DateTime.MinValue,
                                PlanOutQty = r.plan_out_qty != null ? Convert.ToDouble(r.plan_out_qty) : 0,
                                FactOutQty = r.fact_out_qty != null ? Convert.ToDouble(r.fact_out_qty) : 0,

                                // Сохраняем исходное нормативное время этой конкретной строки фазы!
                                NormTimeMinutes = r.normtime != null ? Convert.ToDouble(r.normtime) : 0,

                                IdOrderHead = r.id_order_head != null ? Convert.ToInt32(r.id_order_head) : 0,
                                IsIdleTime = string.IsNullOrEmpty(r.order_num?.ToString()) || r.ord == null
                            };

                            // 🌟 ВОССТАНАВЛИВАЕМ НАЗВАНИЕ ПРОСТОЯ: 
                            // Если это технологический простой (нет номера заказа), забираем его имя из order_name или смежных полей
                            if (card.IsIdleTime)
                            {
                                card.IdleTimeName = !string.IsNullOrEmpty(card.OrderName) ? card.OrderName : "Технологический простой";
                            }

                            return card;
                        }).ToList();

                        // -----------------------------------------------------------------
                        // ШАГ 2: Подтягиваем списки технологических операций (proc_rpt_job_plan_oper_list)
                        // -----------------------------------------------------------------
                        var operParams = new { dtDateBeg = startDate, dtDateEnd = endDate, nObjectID = externalEquipId, nObjectType = 10 };
                        var rawOpers = await conn.QueryAsync("exec proc_rpt_job_plan_oper_list @dtDateBeg, @dtDateEnd, @nObjectID, @nObjectType", operParams);

                        if (rawOpers != null)
                        {
                            // Кэш по ключу "id_man_planjob + _ + main_suboper" (main_suboper соответствует нашему разделению фаз ord)
                            var operCache = new Dictionary<string, dynamic>();
                            foreach (var op in rawOpers)
                            {
                                if (op.id_man_planjob != null && op.main_suboper != null)
                                {
                                    string key = $"{op.id_man_planjob}_{op.main_suboper}";
                                    if (!operCache.ContainsKey(key)) operCache[key] = op;
                                }
                            }

                            // Склеиваем доп. свойства (краски, заметки) с каждой строкой фазы
                            foreach (var job in jobsList)
                            {
                                string searchKey = $"{job.IdPlanJob}_{job.Ord}";
                                if (operCache.ContainsKey(searchKey))
                                {
                                    var matchedOp = operCache[searchKey];
                                    job.MainSuboper = Convert.ToInt32(matchedOp.main_suboper);
                                    job.ItemName = matchedOp.itemname?.ToString() ?? string.Empty;
                                    job.OperProp = matchedOp.oper_prop?.ToString() ?? string.Empty;
                                    job.NoteManagerTech = matchedOp.note_manager_tech?.ToString() ?? string.Empty;

                                    // Если время в основной процедуре было нулевым, берем тотал из карточки операции
                                    if (job.NormTimeMinutes <= 0 && matchedOp.normtime_total != null)
                                    {
                                        job.NormTimeMinutes = Convert.ToDouble(matchedOp.normtime_total);
                                    }
                                }
                            }
                        }

                        // Извлекаем уникальные ID для связи штампов и бумаги
                        var uniqueOrderIds = jobsList.Where(j => j.IdOrderHead > 0 && !j.IsIdleTime)
                                                     .Select(j => j.IdOrderHead)
                                                     .Distinct().ToList();

                        // -----------------------------------------------------------------
                        // ШАГ 3: Подтягиваем штампы (proc_rpt_order_tool)
                        // -----------------------------------------------------------------
                        var toolCache = new Dictionary<int, string>();
                        foreach (var orderId in uniqueOrderIds)
                        {
                            var tools = await conn.QueryAsync("exec proc_rpt_order_tool @id_order_head, 1, 16, 1", new { id_order_head = orderId });
                            if (tools != null)
                            {
                                var uniqueTools = tools.Where(t => t.id_norm_operation != null && Convert.ToInt32(t.id_norm_operation) == 12)
                                                       .Where(t => t.tool_name != null)
                                                       .Select(t => t.tool_name.ToString())
                                                       .Distinct().ToList();

                                if (uniqueTools.Any()) toolCache[orderId] = string.Join(", ", uniqueTools);
                            }
                        }

                        foreach (var job in jobsList)
                        {
                            if (toolCache.ContainsKey(job.IdOrderHead)) job.FullToolName = toolCache[job.IdOrderHead];
                        }

                        // -----------------------------------------------------------------
                        // ШАГ 4: Подтягиваем запечатываемый материал (proc_rpt_order_paper_BY)
                        // -----------------------------------------------------------------
                        var paperCache = new Dictionary<string, dynamic>();
                        foreach (var orderId in uniqueOrderIds)
                        {
                            var papers = await conn.QueryAsync("exec proc_rpt_order_paper_BY @id_order_head, 16, 1, 0, 1, 611, 16", new { id_order_head = orderId });

                            if (papers != null)
                            {
                                foreach (var p in papers)
                                {
                                    if (p.sheet_name != null)
                                    {
                                        string key = $"{orderId}"; //_{p.sheet_name.ToString().Trim()}
                                        if (!paperCache.ContainsKey(key)) paperCache[key] = p;
                                    }
                                }
                            }
                        }
                        //Console.WriteLine("paperCache.conut " + paperCache.Count);
                        foreach (var job in jobsList)
                        {
                            if (job.IsIdleTime*//* || string.IsNullOrEmpty(job.ItemName)*//*) continue;

                            string searchKey = $"{job.IdOrderHead}"; //_{job.ItemName.Trim()}
                            if (paperCache.ContainsKey(searchKey))
                            {
                                var matchedPaper = paperCache[searchKey];
                                job.PaperName = matchedPaper.paper_name?.ToString() ?? string.Empty;
                                job.FormatPaper = matchedPaper.format_paper?.ToString() ?? string.Empty;
                                job.FormatCutting = matchedPaper.format_cutting?.ToString() ?? string.Empty;
                                job.CuttingPart = matchedPaper.cuttingpart != null ? Convert.ToDouble(matchedPaper.cuttingpart) : 0;
                                job.Density = matchedPaper.density != null ? Convert.ToDouble(matchedPaper.density) : 0;
                                job.Width = matchedPaper.width != null ? Convert.ToDouble(matchedPaper.width) : 0;
                            }
                        }

                        return jobsList;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CRITICAL ERROR: GetProductionJobsAsync -> " + ex.Message);
                    return new List<ProductionJobCard>();
                }
            }
        }*/



        public async Task<List<ProductionJobCard>> GetProductionJobsAsyncOLDDDDDDD(DateTime startDate, DateTime endDate, string externalConnString)
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
                  AND plan_out_qty IS NOT NULL
                  AND man_planjob.date_begin <= @EndDate 
                  AND man_planjob.date_end >= @StartDate
                ORDER BY man_planjob.date_begin ASC;
                ";

            // Жесткий лимит ожидания ответа от удаленной сети — 4 секунды
            using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(700)))
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
                                    CustomerName = (string)baseRow.UlName,
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
                                else
                                {
                                    card.RunNormTime = (double)baseRow.NormTime;
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
        public async Task<List<EquipmentStaffingPeriodRow>> GetEquipmentStaffingForPeriodAsync(DateTime startDate, DateTime endDate, int equipmentId, string externalConnString)
        {
            using (var conn = new MySqlConnection(externalConnString))
            {
                var result = await conn.QueryAsync<EquipmentStaffingPeriodRow>(
                    "GetEquipmentStaffingForPeriod",
                    new
                    {
                        p_start_date = startDate.Date,
                        p_end_date = endDate.Date,
                        p_equipment_id = equipmentId
                    },
                    commandType: CommandType.StoredProcedure
                );
                return result.ToList();
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
