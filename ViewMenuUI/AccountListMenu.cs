using System;
using MyLibrary;
using wdt_Assignment1_s3757573.Managers;
using Microsoft.Extensions.Configuration;
using wdt_Assignment1_s3757573.ExceptionBox;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * The classhandle all the operations in the AccountList interface.
 */

namespace wdt_Assignment1_s3757573.ViewMenuUI
{
    public class AccountListMenu
    {

        public Customer Customer { get; set; }
        private readonly TransactionType TransactionType;

        public AccountListMenu(Customer customer, TransactionType transactionType)
        {
            Customer = customer;
            TransactionType = transactionType;
        }


        /*
         * UI for the AccountList menu.
         */
        public void AccountListMenuRun()
        {
            //Each time the menu runs, the latest Customer information is reread from the database.
            //If not, the user will not be able to see the Balance change when they return to the menu.
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

            //display the menu for user select.
            Console.WriteLine();
            Console.WriteLine($"=============== Select Account ==================");
            Console.WriteLine();

            //Use the Transaction Type which pass from the MainMenu to display the different UI
            if (TransactionType.Equals(TransactionType.D))
            {
              Console.WriteLine($"Please select an Account to deposit money into: ");
            }
            else if(TransactionType.Equals(TransactionType.W))
            {
              Console.WriteLine($"Please select an Account to withdrow money: ");
            }
            else 
            {
              Console.WriteLine($"Please select an Account to transfer money: ");
            }
            Console.WriteLine();


            //Customer's account information is dynamically arranged and displayed, and options are provided
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

                if ( i == Customer.Accounts.Count)
                {
                    Console.WriteLine((i+1)+$".  Return to Main Menu ");
                }
            }
            Console.WriteLine();

            while (true)
            {
                var input = Console.ReadLine();

                //Validate user input and verify that the Account exists.
                if (!int.TryParse(input, out var checkInput) || UsefulFunction.CheckRange(checkInput, 1, Customer.Accounts.Count+1) == false)
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
                        //Because both withdraw and deposit use the same AccountList Menu,
                        //we need to judge the actions of the users first.
                        if (TransactionType.Equals(TransactionType.D))
                        {
                            Console.Clear();

                            //(checkinput-1) is the index of this Account in the List.
                            //This means that the user enters the Position of the Account in the list.
                            DepositRun(checkInput-1);
                            break;
                        }
                        else if (TransactionType.Equals(TransactionType.W))
                        {
                            Console.Clear();
                            Withdraw(checkInput-1);
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            Transfer(checkInput-1);
                            break;
                        }
                    }
                }
            }
        }


        /*
         * return to Main Menu.
         */
        public void ReturnToMainMenu()
        {
            Console.Clear();

            MainMenu mainMenu = new MainMenu(Customer);
            mainMenu.MainMenuRun();
        }


        /*
         * The deposit method in Account is called to implement the deposit.
         */
        public void DepositRun(int index)
        {
                Console.WriteLine($"=============== Deposit ==================");
                Console.WriteLine();
                Console.WriteLine($"Your current balance in this account is " + Customer.Accounts[index].Balance);
                Console.WriteLine();
                Console.WriteLine($"Please enter the amount: ");

                while (true)
                {
                   var inputAmount = Console.ReadLine();

                   //check the user input.
                   if (!decimal.TryParse(inputAmount, out var ccorrectInput))
                   {
                       Console.WriteLine($"Invalid input. Please enter again.");
                       continue;  
                   }
                   else
                   {
                       string command = "ATM Server : Deposit money into account";
                       int destinationAccount = 0;
                       
                       try
                       {
                          Customer.Accounts[index].Deposit(ccorrectInput, destinationAccount, command, "D");
                          break;
                       }
                       catch(AccountBalanceException ae)
                       {
                          Console.WriteLine(ae.Message);
                          break;
                       }
                       catch(Exception e)
                       {
                          Console.WriteLine(e.Message);
                          break;
                       }
                    }
                }
                Console.WriteLine();
                Console.WriteLine($"your balance in this account now is " + Customer.Accounts[index].Balance);
                Console.WriteLine($"Press any key to return to selction menu");
                Console.ReadKey();
                Console.Clear();
                AccountListMenuRun();
        }



        /*
         * The withdraw method in Account is called to implement the withdraw.
         */
        public void Withdraw(int index)
        {
            Console.WriteLine($"=============== Withdraw ==================");
            Console.WriteLine();
            Console.WriteLine($"Your current balance in this account is " + Customer.Accounts[index].Balance);
            Console.WriteLine();
            Console.WriteLine($"Please enter the amount: ");

            while (true)
            {
                var inputAmount = Console.ReadLine();

                //check the user input.
                if (!decimal.TryParse(inputAmount, out var correctInput))
                {
                    Console.WriteLine($"Invalid input. Please enter again.");
                    continue;
                }
                else
                {
                    try
                    {
                        string command = "ATM Server : Withdraw money from account";
                        int destinationAccount = 0;
                        decimal chargesFee = new decimal(0.1);
                        Customer.Accounts[index].Withdraw(correctInput, chargesFee, destinationAccount, command, "W");
                        Console.WriteLine();
                        break;
                    }
                    catch (AccountBalanceException ae)
                    {
                        Console.WriteLine(ae.Message);
                        break;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
            }
            Console.WriteLine($"your balance in this account now is " + Customer.Accounts[index].Balance);
            Console.WriteLine($"Press any key to return to selction menu");
            Console.ReadKey();
            Console.Clear();
            AccountListMenuRun(); 
        }



        /*
         * The transfer method in Account is called to implement the transfer.
         */
        public void Transfer(int index)
        {
            Console.WriteLine($"=============== Transfer ==================");
            Console.WriteLine();
            Console.WriteLine($"Your current balance in this account is " + Customer.Accounts[index].Balance);
            Console.WriteLine();
            Console.WriteLine($"Please enter the Account number that you wish to transfer money into: ");

            while (true)
            {
                var inputAccountNumber = Console.ReadLine();

                if (!int.TryParse(inputAccountNumber, out var correctAccountNumber))
                {
                    Console.WriteLine($"Invalid input. Please enter again.");
                    continue;
                }
                else
                {
                    Console.WriteLine($"Please enter the amount that you wana transfer: ");
                    while (true)
                    {
                        var inputAmount = Console.ReadLine();
                        if (!decimal.TryParse(inputAmount, out var correctAmount))
                        {
                            Console.WriteLine($"Invalid Amount, please enter again.");
                            continue;
                        }
                        else
                        {
                            try
                            {
                                //Reads all Accounts from the database.
                                //Moreover, the Account entered by the user is compared with the Account entered by the user.
                                //If the same Account is not found, the Account entered by the user does not exist.
                                var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
                                var connectionKey = configuration["ConnectionString"];
                                AccountManager accountManager = new AccountManager(connectionKey);
                                var AccountList = accountManager.GetAccountsByAccountNumber(correctAccountNumber);

                                //if AccountList is empty which mean we cant find this account number in database.
                                if (AccountList.Count == 0)
                                {
                                    throw new AccountNoFundException($"Account no fund, please check account number.");
                                }
                                else
                                {
                                    //if everything is fine, we call the transfer method to start transfer.
                                    Console.WriteLine($"Add description for Transaction(optional): ");
                                    Console.WriteLine();
                                    var command = Console.ReadLine();
                                    Customer.Accounts[index].Transfer(correctAmount, AccountList[0], command);
                                    break;
                                }

                            }
                            catch (AccountNoFundException a)
                            {
                                Console.WriteLine(a.Message);
                                break;
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                                break;
                            }
                        }
                    }
                    
                }
                Console.WriteLine();
                Console.WriteLine($"your balance in this account now is " + Customer.Accounts[index].Balance);
                Console.WriteLine($"Press any key to return to selction menu");
                Console.ReadKey();
                Console.Clear();
                AccountListMenuRun();
            } 
        }
    }
}
