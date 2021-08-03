using System;
using MyLibrary;
using wdt_Assignment1_s3757573.ExceptionBox;
using wdt_Assignment1_s3757573.Managers;
using Microsoft.Extensions.Configuration;


/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * The class handle all ATMtransactionMenu.
 */

namespace wdt_Assignment1_s3757573.ViewMenuUI
{
    public class ATMtransactionMenu
    {
        public Customer Customer { get; set; }

        public ATMtransactionMenu(Customer customer)
        {
            Customer = customer;
        }


        /*
         * To display the ATM menu.
         */
        public void ATMtransactionMenuRun()
        {

            //the latest Customer information is reread from the database.
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
            Console.WriteLine($"=============== ATM Menu ==================");
            Console.WriteLine();
            Console.WriteLine($"Please select an option from the following: ");
            Console.WriteLine();
            Console.WriteLine($"1. -- Deposit Money  ");
            Console.WriteLine($"2. -- Withdraw Money  ");
            Console.WriteLine($"3. -- Return to Main Menu ");
            Console.WriteLine();

            while (true)
            {
                var input = Console.ReadLine();

                //check the user input.
                if (!int.TryParse(input, out var checkInput) || UsefulFunction.CheckRange(checkInput, 1, 3) == false)
                {
                    Console.WriteLine($"Invalid input. Please enter again.");
                    continue;
                }
                else
                {
                    switch (checkInput)
                    {
                        case 1:  Deposite(); break;
                        case 2:  Withdraw(); break;
                        case 3:  ReturnMainMenu(); break;
                        default: throw new MenuSelectException("Invalid input");

                    }
                }
            }
        }


        /*
         * To display the Main Menu.
         */
        public void ReturnMainMenu()
        {
            Console.Clear();

            MainMenu mainMenu = new MainMenu(Customer);
            mainMenu.MainMenuRun();
        }


        /*
         * To display the AccountList menu.
         */
        public void Deposite()
        {
            Console.Clear();

            //When we start running the AccountList menu,
            //we need to pass a TransactionType to tell AccountList how to call the following method.
            AccountListMenu alm = new AccountListMenu(Customer,TransactionType.D);
            alm.AccountListMenuRun();
        }


        /*
         * To display the AccountList menu.
         */
        public void Withdraw()
        {
            Console.Clear();
            AccountListMenu alm = new AccountListMenu(Customer, TransactionType.W);
            alm.AccountListMenuRun();
        }  
    }
}
