using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DataProviderFactory.Models;

namespace DataProviderFactory.DataOperations
{
    // ReSharper disable once InconsistentNaming
    //todo make it IDisposable and try - catch exceptions
    public sealed class InventoryDAL
    {
        #region Fields

        #endregion


        #region Properties

        #endregion


        #region Command

        #endregion


        #region Constructor

        private readonly String connection_string;

        public InventoryDAL() : this(connection_string: @"Data Source = (localdb)\mssqllocaldb;Integrated Security=true; Initial Catalog=AutoLot")
        {
        }

        public InventoryDAL(String connection_string) => this.connection_string = connection_string;

        #endregion


        #region Events

        #endregion


        #region Privats methods

        private SqlConnection sql_connection = null;

        private void OpenConnection()
        {
            this.sql_connection = new SqlConnection {ConnectionString = this.connection_string};
            this.sql_connection.Open();
        }

        private void CloseConnection()
        {
            if (this.sql_connection?.State != ConnectionState.Closed)
            {
                this.sql_connection?.Close();
            }
        }

        #endregion


        #region Public methods

        public List<Car> GetAllInventory()
        {
            OpenConnection();
            // Здесь будут храниться записи.
            List<Car> inventory = new List<Car>();
            // Подготовить объект команды,
            String sql = "Select * From Inventory";
            using (SqlCommand command = new SqlCommand(cmdText: sql, connection: this.sql_connection))
            {
                command.CommandType = CommandType.Text;
                SqlDataReader data_reader = command.ExecuteReader(behavior: CommandBehavior.CloseConnection);
                while (data_reader.Read())
                {
                    inventory.Add(
                        item: new Car
                        {
                            CarId = (Int32)data_reader[name: "CarId"],
                            Color = (String)data_reader[name: "Color"],
                            Make = (String)data_reader[name: "Make"],
                            PetName = (String)data_reader[name: "PetName"]
                        });
                }

                data_reader.Close();
            }

            return inventory;
        }

        public Car GetCar(Int32 id)
        {
            OpenConnection();
            Car car = null;
            String sql = $"Select * From Inventory where CarId = {id}";
            using (SqlCommand command = new SqlCommand(cmdText: sql, connection: this.sql_connection))
            {
                command.CommandType = CommandType.Text;
                SqlDataReader data_reader = command.ExecuteReader(behavior: CommandBehavior.CloseConnection);
                while (data_reader.Read())
                {
                    car = new Car
                    {
                        CarId = (Int32)data_reader[name: "CarId"],
                        Color = (String)data_reader[name: "Color"],
                        Make = (String)data_reader[name: "Make"],
                        PetName = (String)data_reader[name: "PetName"]
                    };
                }

                data_reader.Close();
                return car;
            }
        }

        public void InsertAuto(String color, String make, String pet_name)
        {
            OpenConnection();
            // Сформатировать и выполнить оператор SQL.
            String sql = $"Insert Into Inventory (Make, Color, PetName) Values('{make}', '{color}’, '{pet_name}')";
            // Выполнить, используя наше подключение.
            using (SqlCommand command = new SqlCommand(cmdText: sql, connection: this.sql_connection))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }


        public void InsertAuto(Car car)
        {
            OpenConnection();
            // Сформатировать и выполнить оператор SQL.
            String sql = "Insert Into Inventory" +
                         "(Make, Color, PetName) Values" +
                         "(@Make, @Color, @PetName)";
            // Выполнить, используя наше подключение.
            using (SqlCommand command = new SqlCommand(cmdText: sql, connection: this.sql_connection))
            {
                // Заполнить коллекцию параметров.
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@Make",
                    Value = car.Make,
                    SqlDbType = SqlDbType.Char,
                    Size = 10
                };
                command.Parameters.Add(value: parameter);

                parameter = new SqlParameter
                {
                    ParameterName = "@Color",
                    Value = car.Color,
                    SqlDbType = SqlDbType.Char,
                    Size = 10
                };
                command.Parameters.Add(value: parameter);

                parameter = new SqlParameter
                {
                    ParameterName = "@PetName",
                    Value = car.PetName,
                    SqlDbType = SqlDbType.Char,
                    Size = 10
                };
                command.Parameters.Add(value: parameter);

                command.ExecuteNonQuery();
                CloseConnection();
            }
        }

        public void DeleteCar(Int32 id)
        {
            OpenConnection();

            // Получить идентификатор автомобиля, подлежащего удалению,
            //и удалить запись о нем.
            String sql = $"Delete from Inventory where CarId = '{id}'";
            using (SqlCommand command = new SqlCommand(cmdText: sql, connection: this.sql_connection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception(message: "Sorry! That car is on order!", innerException: ex);
                    // Этот автомобиль заказан!
                    throw error;
                }
            }

            CloseConnection();
        }


        public void UpdateCarPetName(Int32 id, String new_pet_name)
        {
            OpenConnection();
            // Получить идентификатор автомобиля для модификации дружественного имени,
            String sql = $"Update Inventory Set PetName = '{new_pet_name}' Where CarId = ’{id}'";
            using (SqlCommand command = new SqlCommand(cmdText: sql, connection: this.sql_connection))
            {
                command.ExecuteNonQuery();
            }

            CloseConnection();
        }


        public String LookUpPetName(Int32 car_ld)
        {
            OpenConnection();
            String car_pet_name;
            // Установить имя хранимой процедуры.
            using (SqlCommand command = new SqlCommand(cmdText: "GetPetName", connection: this.sql_connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                // Входной параметр.
                SqlParameter param = new SqlParameter
                {
                    ParameterName = "@carId",
                    SqlDbType = SqlDbType.Int,
                    Value = car_ld,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(value: param);
                // Выходной параметр,
                param = new SqlParameter
                {
                    ParameterName = "@petName",
                    SqlDbType = SqlDbType.Char,
                    Size = 10,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(value: param);
                // Выполнить хранимую процедуру,
                command.ExecuteNonQuery();
                // Возвратить выходной параметр.
                car_pet_name = (String)command.Parameters[parameterName: "@petName"].Value;
                CloseConnection();
            }

            return car_pet_name;
        }

        public void ProcessCreditRisk(Boolean throw_ex, Int32 custId)
        {
            OpenConnection();
            // Первым делом найти текущее имя по идентификатору клиента,
            String f_name;
            String l_name;
            var cmd_select = new SqlCommand($"Select * from Customers where CustId = {custId}", this.sql_connection);
            using (var data_reader = cmd_select.ExecuteReader())
            {
                if (data_reader.HasRows)
                {
                    data_reader.Read();
                    f_name = (String)data_reader["FirstName"];
                    l_name = (String)data_reader["LastName"];
                }
                else
                {
                    CloseConnection();
                    return;
                }
            }

            // Создать объекты команд, которые представляют каждый шаг операции,
            var cmd_remove = new SqlCommand($"Delete from Customers where CustId = {custId}", this.sql_connection);
            var cmd_insert = new SqlCommand("Insert Into CreditRisks" + $"(FirstName, LastName) Values('{f_name}', '{l_name}')", this.sql_connection);
            // Это будет получено из объекта подключения.
            SqlTransaction tx = null;
            try
            {
                tx = this.sql_connection.BeginTransaction();
                // Включить команды в транзакцию,
                cmd_insert.Transaction = tx;
                cmd_remove.Transaction = tx;
                // Выполнить команды,
                cmd_insert.ExecuteNonQuery();
                cmd_remove.ExecuteNonQuery();
                // Эмулировать ошибку,
                if (throw_ex)
                {
                    // Возникла ошибка, связанная с базой данных! Отказ транзакции...
                    throw new Exception("Sorry! Database error! Tx failed...");
                }

                // Зафиксировать транзакцию!
                tx.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Любая ошибка приведет к откату транзакции.
                // Использовать условную операцию для проверки на предмет null,
                tx?.Rollback();
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion
    }
}