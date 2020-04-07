using System;
using System.Data.SqlClient;
using static System.Console;

namespace DataProviderFactory
{
    class Program
    {
        static void Main(String[] args)
        {
            WriteLine(value: "**** Test with Data Provider Factories ****\n");
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot";
                connection.Open();
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
    }
}