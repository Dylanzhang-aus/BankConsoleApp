using System;
using wdt_Assignment1_s3757573.Managers;
using Microsoft.Extensions.Configuration;
using MyLibrary;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * The class handle all System Menu.
 */

namespace wdt_Assignment1_s3757573.ViewMenuUI
{
    public class MysatementMenu
    {
        public Customer Customer { get; set; }

        public MysatementMenu(Customer customer)
        {
            Customer = customer;
        }


        /*
         * To display the My statement menu.
         */
        public void MysatementMenuRun()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
            var connectionKey = configuration["ConnectionString"];
            CustomerManager customerManager = new CustomerManager(connectionKey);
            foreach (var c in customerManager.CustomerList)
            {
                if (Customer.CustomerID == c.CustomerID)
                {
                    Customer = c;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"=============== Select Account ==================");
            Console.WriteLine();
            Console.WriteLine($"Please select an Account to show the statement: ");
            Console.WriteLine();

            //Display the current menu dynamically because the number of accounts is variable.
            for (int i = 1; i <= Customer.Accounts.Count; i++)
            {
                if (Customer.Accounts[i - 1].AccountType.Equals("S"))
                {
                    Console.WriteLine(i + $".  Saving Account -- " + Customer.Accounts[i - 1].AccountNumber);
                }
                else
                {
                    Console.WriteLine(i + $".  Checking Account -- " + Customer.Accounts[i - 1].AccountNumber);
                }

                if (i == Customer.Accounts.Count)
                {
                    Console.WriteLine((i + 1) + $".  Return to Main Menu ");
                }
            }
            Console.WriteLine();

            while (true)
            {
                var input = Console.ReadLine();
                if (!int.TryParse(input, out var checkInput) || UsefulFunction.CheckRange(checkInput, 1, Customer.Accounts.Count + 1) == false)
                {
                    Console.WriteLine($"Invalid input. Please enter again.");
                    continue;
                }
                else
                {                   
                    if (checkInput == Customer.Accounts.Count + 1)
                    {
                        ReturnToMainMenu();
                        break;
                    }
                    else
                    {
                        ShowStatement(checkInput - 1);
                        break;
                    }
                }
            }
        }


        /*
         * Come back to Main menu.
         */
        public void ReturnToMainMenu()
        {
            Console.Clear();

            MainMenu mainMenu = new MainMenu(Customer);
            mainMenu.MainMenuRun();
        }



        /*
         * To display my statement table.
         * The displayStatement method on Account is called directly to display the Table
         */
        public void ShowStatement(int index)
        {
            Console.Clear();
            Console.WriteLine("$Account  --  " + Customer.Accounts[index].AccountNumber + "  " +
                              "---  Current balance : " + Customer.Accounts[index].Balance);
            Console.WriteLine();
            Customer.Accounts[index].DisplayStatement();
            Console.WriteLine();
            Console.Clear();
            MainMenu mainMenu = new MainMenu(Customer);
            mainMenu.MainMenuRun();
        }
    }
}
