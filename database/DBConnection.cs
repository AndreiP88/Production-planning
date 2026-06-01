using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;
using System.Threading;

namespace database
{
    public class DBConnection
    {
        public static MySqlConnection GetMySQLConnection()
        {
            MySqlConnection connect = null;
            ConnectionParameter parameters = new ConnectionParameter();

            bool reconnectionRequired = false;
            int reconnectCount = 0;
            int reconnectLimit = 5;

            string host = parameters.Host;
            int port = parameters.Port;
            string database = parameters.Database;
            string username = parameters.Username;
            string password = parameters.Password;

            do
            {
                try
                {
                    connect = DBSQLUtils.GetMySQLConnection(host, port, database, username, password);

                    reconnectionRequired = false;
                }
                catch (Exception ex)
                {
                    reconnectCount++;

                    //LogException.WriteLine("GetDBConnection " + reconnectCount + " of " + reconnectLimit + "\n" + ex.Message + "; " + ex.StackTrace);

                    if (reconnectCount <= reconnectLimit)
                    {
                        reconnectionRequired = true;
                        Thread.Sleep(500);
                    }
                    else
                    {
                        //Application.Exit();
                    }
                }
            }
            while (reconnectionRequired);

            return connect;
        }

        public bool IsServerConnected(string host, int port, string database, string username, string password)
        {
            using (MySqlConnection Connect = DBSQLUtils.GetMySQLConnection(host, port, database, username, password))
            {
                try
                {
                    Connect.Open();
                    Connect.Close();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public static SqlConnection GetDBConnection()
        {
            string host = "SRV-ACS\\DSACS";
            string database = "asystem";
            string username = "ds";
            string password = "1";

            return DBSQLUtils.GetDBConnection(host, database, username, password);
        }

        public static SqlConnection GetDBConnection(string host, string database, string username, string password)
        {
            /*string host = "25.21.38.172";
            int port = 3309;
            string database = "order_manager";
            string username = "oxyfox";
            string password = "root";*/

            return DBSQLUtils.GetDBConnection(host, database, username, password);
        }

        public bool IsServerConnected()
        {
            using (SqlConnection Connect = GetDBConnection())
            {
                try
                {
                    Connect.Open();
                    Connect.Close();
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
    }
}
