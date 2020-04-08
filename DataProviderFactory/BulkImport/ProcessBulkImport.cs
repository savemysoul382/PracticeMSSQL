using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataProviderFactory.BulkImport
{
    public static class ProcessBulkImport
    {
        private static SqlConnection sql_connection = null;
        private static readonly String connection_string = @"Data Source = (localdb)\mssqllocaldb;Integrated Security=true;Initial Catalog=AutoLot ";

        private static void OpenConnection()
        {
            sql_connection = new SqlConnection {ConnectionString = connection_string};
            sql_connection.Open();
        }

        private static void CloseConnection()
        {
            if (sql_connection?.State != ConnectionState.Closed)
            {
                sql_connection?.Close();
            }
        }

        public static void ExecuteBulkImport<T>(IEnumerable<T> records, String table_name)
        {
            OpenConnection();
            using (SqlConnection conn = sql_connection)
            {
                SqlBulkCopy be = new SqlBulkCopy(conn)
                {
                    DestinationTableName = table_name
                };
                var data_reader = new MyDataReader<T>(records.ToList());
                try
                {
                    be.WriteToServer(data_reader);
                }
                catch (Exception ex)
                {
                    // Здесь должно что-то делаться.
                }
                finally
                {
                    CloseConnection();
                }
            }
        }
    }
}