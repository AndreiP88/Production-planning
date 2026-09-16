namespace database
{
    public class ConnectionParameter
    {
        public string Host = "localhost";
        public readonly int Port = 3309;
        public string Database = "workplan";
        public string Username = "oxyfox";
        public string Password = "root";

        public ConnectionParameter()
        {

        }

        public string GetMySQLConnectionString()
        {
            string connString = "Server=" + Host + ";Database=" + Database + ";port=" + Port + ";User Id=" + Username + ";password=" + Password;

            return connString;
        }

        public string GetSQLServerConnectionString()
        {
            string host = "SRV-ACS\\DSACS";
            string database = "asystem";
            string username = "ds";
            string password = "1";

            string connString = "Data Source = " + host + "; Initial Catalog = " + database + "; Persist Security Info = True; User ID = " + username + "; Password = " + password;

            return connString;
        }
    }
}
