namespace database
{
    public class ConnectionParameter
    {
        public string Host = "versorpc";
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
    }
}
