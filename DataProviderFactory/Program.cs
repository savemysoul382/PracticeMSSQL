using System;
using System.Collections.Generic;
using DataProviderFactory.BulkImport;
using DataProviderFactory.DataOperations;
using DataProviderFactory.Models;

namespace DataProviderFactory
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.WriteLine(" ************** Do Bulk Copy ************** ");
            var cars = new List<Car>
            {
                new Car() {Color = "Blue", Make = "Honda", PetName = "MyCarl"},
                new Car() {Color = "Red", Make = "Volvo", PetName = "MyCar2"},
                new Car() {Color = "White", Make = "VW", PetName = "МуСагЗ"},
                new Car() {Color = "Yellow", Make = "Toyota", PetName = "MyCar4"}
            };
            ProcessBulkImport.ExecuteBulkImport(cars, "Inventory");
            InventoryDAL dal = new InventoryDAL();
            var list = dal.GetAllInventory();
            Console.WriteLine(" ************** All Cars ************** ");
            Console.WriteLine("CarId\tMake\tColor\tPet Name");
            foreach (var item in list)
            {
                Console.WriteLine($"{item.CarId}\t{item.Make}\t{item.Color}\t{item.PetName}");
            }

            Console.WriteLine();
        }
    }
}