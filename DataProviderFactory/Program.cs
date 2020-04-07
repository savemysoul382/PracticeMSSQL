using System;
using System.Linq;
using DataProviderFactory.DataOperations;
using DataProviderFactory.Models;
using static System.Console;

namespace DataProviderFactory
{
    class Program
    {
        static void Main(String[] args)
        {
            WriteLine(value: "**** Test with Data Readers ****\n");

            InventoryDAL dal = new InventoryDAL();
            var list = dal.GetAllInventory();
            Console.WriteLine(" ************** Аll cars **********");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            foreach (var itm in list)
            {
                Console.WriteLine($" {itm.CarId} \t {itm.Make} \t {itm.Color} \t {itm.PetName} ");
            }

            Console.WriteLine();
            var car = dal.GetCar(list.OrderBy(x => x.Color).Select(x => x.CarId).First());
            Console.WriteLine(" ************** First Car By Color ************** ");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            Console.WriteLine($"{car.CarId}\t{car.Make}\t{car.Color}\t{car.PetName}");
            try
            {
                dal.DeleteCar(5);
                Console.WriteLine("Car deleted.");
                // Запись об автомобиле удалена
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception occurred: {ex.Message}");
                // Возникло исключение
            }

            dal.InsertAuto(
                car: new Car
                {
                    Color = "Blue", Make = "Pilot", PetName = "TowMonster"
                });
            list = dal.GetAllInventory();
            var new_car = list.First(x => x.PetName.Trim() == "TowMonster");
            Console.WriteLine(" ************** New Car ************** ");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            Console.WriteLine($"{new_car.CarId}\t{new_car.Make}\t{new_car.Color}\t{new_car.PetName}");
            dal.DeleteCar(new_car.CarId);
            var petName = dal.LookUpPetName(car.CarId);
            Console.WriteLine(" ************** New Car ************** ");
            Console.WriteLine($"Car pet name: {petName}");
            Console.Write("Press enter to continue...");
            // Для продолжения нажмите <Enter>...
            Console.ReadLine();
        }
    }
}