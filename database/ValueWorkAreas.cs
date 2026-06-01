using data;
using database;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Production_planning
{
    public class ValueWorkAreas
    {
        public ValueWorkAreas()
        {

        }

        public List<WorkAreas> GetWorkAreasList()
        {
            List<WorkAreas> areas = new List<WorkAreas>();

            using (MySqlConnection Connect = DBConnection.GetMySQLConnection())
            {
                Connect.Open();
                MySqlCommand Command = new MySqlCommand
                {
                    Connection = Connect,
                    CommandText = @"SELECT id, name, sort_order 
                                    FROM work_areas 
                                    ORDER BY sort_order ASC;"
                };
                DbDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    areas.Add(new WorkAreas(Convert.ToInt32(sqlReader["id"]), sqlReader["name"].ToString(), Convert.ToInt32(sqlReader["sort_order"])));
                }

                Connect.Close();
            }
            return areas;
        }
    }
}
