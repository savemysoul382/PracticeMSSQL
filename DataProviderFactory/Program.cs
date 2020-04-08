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
            Console.WriteLine("***** Simple Transaction Example *****\n");
            // Простой способ позволить транзакции успешно завершиться или отказать,
            Boolean throw_ex = true;
            Console.Write("Do you want to throw an exception (Y or N) : ");
            // Хотите ли вы сгенерировать исключение?
            var user_answer = Console.ReadLine();
            if (user_answer?.ToLower() == "n")
            {
                throw_ex = false;
            }

            var dal = new InventoryDAL();
            // Обработать клиента 1 - ввести идентификатор клиента,
            // подлежащего перемещению.
            dal.ProcessCreditRisk(throw_ex, 2);
            Console.WriteLine("Check CreditRisk table for results");
            // Результаты ищите в таблице CreditRisk
            Console.ReadLine();
        }
    }
}