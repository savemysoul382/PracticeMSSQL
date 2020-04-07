using System;
using System.Data.SqlClient;
using static System.Console;

namespace DataProviderFactory
{
    class Program
    {
        static void Main(String[] args)
        {
            WriteLine(value: "**** Test with Data Readers ****\n");

            // Создать строку подключения с помощью объекта построителя.
            var cn_string_builder = new SqlConnectionStringBuilder
            {
                InitialCatalog = "AutoLot",
                DataSource = @"(localdb)\mssqllocaldb",
                ConnectTimeout = 30,
                IntegratedSecurity = true
            };

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = cn_string_builder.ConnectionString;
                connection.Open();
                ShowConnectionStatus(connection);
                // Создать объект команды SQL.
                String sql = "Select * From Inventory";
                SqlCommand my_command = new SqlCommand(sql, connection);
                // Получить объект чтения данных с помощью ExecuteReader().
                using (SqlDataReader data_reader = my_command.ExecuteReader())
                {
                    // Пройти в цикле по результатам,
                    while (data_reader.Read())
                    {
                        WriteLine($"->Make: {data_reader["Make"]}, PetName: {data_reader["PetName"]},Color:{data_reader["Color"]}.");
                    }
                }
            }

            ReadLine();
        }

        private static void ShowConnectionStatus(SqlConnection connection)
        {
            // Вывести различные сведения о текущем объекте подключения.
            WriteLine("*****Info about your connection * ****");
            WriteLine($"Database location: {connection.DataSource}"); // Местоположение базы данных
            WriteLine($"Database name: {connection.Database}"); // Имя базы данных
            WriteLine($"Timeout: {connection.ConnectionTimeout}"); // Таймаут
            WriteLine($"Connection state: {connection.State}\n"); // Состояние
        }
    }
}