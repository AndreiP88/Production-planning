using Newtonsoft.Json.Linq;
using System;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace database
{
    public class ConnectionParameter
    {
        private readonly string _configFilePath = "settings.json";

        private string GetConnectionStringFromJson(string key)
        {
            if (!File.Exists(_configFilePath))
            {
                throw new FileNotFoundException($"Конфигурационный файл настроек не найден по пути: {_configFilePath}");
            }

            try
            {
                string jsonText = File.ReadAllText(_configFilePath);
                var jObject = JObject.Parse(jsonText);

                string connString = jObject["ConnectionStrings"]?[key]?.ToString();
                if (string.IsNullOrEmpty(connString))
                {
                    throw new Exception($"Ключ строки подключения '{key}' не найден внутри секции ConnectionStrings в settings.json.");
                }

                return connString;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка чтения конфигурации JSON: {ex.Message}");
            }
        }

        public string GetMySQLConnectionString() => GetConnectionStringFromJson("MySQLConnection");
        public string GetSQLServerConnectionString() => GetConnectionStringFromJson("SQLServerConnection");
    }
}
