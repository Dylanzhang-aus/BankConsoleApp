using System;
using wdt_Assignment1_s3757573.WebServer;
using MyLibrary;
using Microsoft.Extensions.Configuration;
using wdt_Assignment1_s3757573.ExceptionBox;
using wdt_Assignment1_s3757573.Managers;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * The class handle all Main menu.
 */

namespace wdt_Assignment1_s3757573.ViewMenuUI
{
    public class MainMenu
    {
        public Customer Customer { get; set; }

        public MainMenu(Customer customer)
        {
            Customer = customer;
        }



        /*
         * To display the Main menu.
         */
        public void MainMenuRun()
        {
            //loading the customer from database
            var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
            var connectionKey = configuration["ConnectionString"];
            CustomerManager customerManager = new CustomerManager(connectionKey);
            foreach(var c in customerManager.CustomerList)
            {
               if(Customer.CustomerID == c.CustomerID)
                {
                    Customer = c;
                }
            }

            Console.WriteLine($"         Welcome!  "+Customer.Name);
            Console.WriteLine();
            Console.WriteLine($"=============== Main Menu ==================");
            Console.WriteLine();
            Console.WriteLine($"Please select an option from the following: ");
            Console.WriteLine();
            Console.WriteLine($"1. -- ATM Transaction  ");
            Console.WriteLine($"2. -- Transfer  ");
            Console.WriteLine($"3. -- My Statements  ");
            Console.WriteLine($"4. -- Logout  ");
            Console.WriteLine($"5. -- Exit  ");
            Console.WriteLine();
 
            while (true)
            {
               var input = Console.ReadLine();
 
               if (!int.TryParse(input, out var checkInput) || UsefulFunction.CheckRange(checkInput,1,5) == false)
               {
                   Console.WriteLine($"Invalid input. Please enter again.");
                   continue;
               }
               else
               {
                   switch (checkInput)
                   {
                      case 1: ATMtransactionMenu(); break;
                      case 2: TransferMenu();  break;
                      case 3: MystatementMenu(); break;
                      case 4: Logout(); break;
                      case 5: Exit(); break;
                      default: throw new MenuSelectException("Invalid input");
                   }
               }
            }
        }



        /*
        * To display the Main menu.
        * Because the logout and exit methods belong only to the Main Menu and do not have any dependencies. 
        * That's why I'm writing static methods here.
        */
        public static void Logout()
        {
            Console.Clear();
            var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
            var connectionKey = configuration["ConnectionString"];
            LoadWebData.LoadingData(connectionKey);
            new LoginMenu(connectionKey).LoginMenuRun();
        }


        /*
         * To display ATM Menu.
         */
        public void ATMtransactionMenu()
        {
            Console.Clear();
 
            ATMtransactionMenu Atm = new ATMtransactionMenu(Customer);
            Atm.ATMtransactionMenuRun();

        }


        /*
         * To display the Accountlist menu.
         */
        public void TransferMenu()
        {
            Console.Clear();

            // need pass transaction type to tell accountlist menu
            // How to call the method.
            AccountListMenu alm = new AccountListMenu(Customer, TransactionType.T);
            alm.AccountListMenuRun();
        }


        /*
         * To display the my statement menu.
         */
        public void MystatementMenu()
        {
            Console.Clear();

            MysatementMenu mm = new MysatementMenu(Customer);

            mm.MysatementMenuRun();
        }


        /*
         * exit from application.
         */
        public static void Exit()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

    }
}
