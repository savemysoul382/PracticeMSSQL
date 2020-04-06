using System;
using System.Configuration;
using System.Data.Common;
using static System.Console;

namespace DataProviderFactory
{
    class Program
    {
        static void Main(String[] args)
        {
            WriteLine(value: "**** Test with Data Provider Factories ****\n");
            String data_provider = ConfigurationManager.AppSettings[name: "provider"];
            String connection_string = ConfigurationManager.AppSettings[name: "connectionString"];

            DbProviderFactory factory = DbProviderFactories.GetFactory(providerInvariantName: data_provider);

            using (DbConnection connection = factory.CreateConnection())
            {
                if (connection == null)
                {
                    ShowError("Connection");
                    return;
                }

                WriteLine($"Your connection object is a: {connection.GetType().Name}");
                connection.ConnectionString = connection_string;
                connection.Open();

                DbCommand command = factory.CreateCommand();
                if (command == null)
                {
                    ShowError("Command");
                    return;
                }
                WriteLine($"Your command object is a: {command.GetType().Name}");
                command.Connection = connection;
                command.CommandText = "Select * From Inventory";

                using (DbDataReader data_reader = command.ExecuteReader())
                {
                    WriteLine($"Your data reader object is a: {data_reader.GetType().Name}");
                    WriteLine("\n***** Current Inventory *****");
                    while (data_reader.Read())
                        WriteLine($"-> Car #{data_reader["CarId"]} is a {data_reader["Make"]}.") ;
                }
                ReadLine();
            }
        }

        private static void ShowError(String object_name)
        {
            WriteLine($"There was an issue creating the {object_name}");
            ReadLine();
        }
    }
}
