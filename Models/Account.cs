using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ConsoleTables;
using wdt_Assignment1_s3757573.ExceptionBox;
using Microsoft.Extensions.Configuration;
using wdt_Assignment1_s3757573.Managers;
using System.Threading.Tasks;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Generate an Account Object. This class is a super class of CAccount and SAccount.
 */

namespace wdt_Assignment1_s3757573
{
    public class Account
    {
        //properties
        [Range(4, 4), Required]
        public int AccountNumber { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public decimal Balance { get; set; }
        public List<Transactions> Transactions { get; set; }


        //Constructor1. This constructor is primarily used to generate the default Account information.
        public Account() { }


        //Constructor2
        public Account(int AccountNumber, string AccountType, int CustomerID, decimal Balance, List<Transactions> Transactions)
        {
            this.AccountNumber = AccountNumber;
            this.AccountType = AccountType;
            this.CustomerID = CustomerID;
            this.Balance = Balance;
            this.Transactions = Transactions;
        }


        /*
         * @premeter amount(How much money the user wants to deposit).
         * @premeter destinationAccount(This parameter is defined as 0 in the deposit method).
         * @premeter command(hard code for deposit).
         * @premeter transactionType(hard code for "D").
         * 
         * The main purpose of this method is to implement the deposit operation. 
         * The extra parameters are added in order to call it properly in the transfer method.
         * The reason of return bool is that I want to know deposit is success or not.
         */
        public bool Deposit(decimal amount, int destinationAccount, string command, string transactionType)
        {
            
            try
            {
                if (amount > 0)
                {
                     Balance += amount;
                     var dateTime = DateTime.Now;
                     Console.WriteLine(amount + $" is deposited successful");

                     var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
                     var connectionKey = configuration["ConnectionString"];
                     AccountManager accountManager = new AccountManager(connectionKey);
                     TransactionManager transactionManager = new TransactionManager(connectionKey);

                     //The reason for using asynchrony here is that when the thread reaches into {if (amount>0)},
                     //the default is that the deposit has been successful,
                     //so the asynchrony is used to make the data update faster.
                     Task updateAccount = accountManager.UpdateAmount(Balance, AccountNumber);
                     Task addTable = transactionManager.AddTransactionTable(new Transactions(AccountNumber, destinationAccount, command, amount, transactionType, dateTime));
                     return true;                       
                 }
                 else
                 {
                     throw new AccountBalanceException($"Negetive and empty amount is not allowed.");
                 }

            }
            catch (AccountBalanceException ae)
            {
                Console.WriteLine(ae.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


       /*
        * @premeter amount(How much money the user wants to withdraw).
        * @premeter destinationAccount(This parameter is defined as 0 in the deposit method).
        * @premeter command(hard code for withdraw).
        * @premeter transactionType(hard code for "W").
        * 
        * This method is for the purpose of withdrawing money. 
        * The main reason for using Virtual and Override is that the minimum balance of the two accounts is different, 
        * so must control the balance as it decreases.
        */
        public virtual bool Withdraw(decimal amount, decimal chargesFee, int destinationAccount, string command, string transactionType) { return false; }



        /*
        * @premeter amount(How much money the user wants to transfer).
        * @premeter destinationAccount.
        * @premeter command(it can by writting by user themself).
        * @premeter transactionType(hard code for "T").
        * 
        * Direct calls to deposit and withdrow methods to realize the whole transfer process 
        */
        public void Transfer(decimal amount, Account destinationAccount, string command)
        {

            //0.2 is the chargesFee for transfer
            decimal chargesFee = new decimal(0.2);

            try
            {
                //Users cannot transfer money from the same account.
                if (AccountNumber == destinationAccount.AccountNumber)
                {
                    throw new TransactionException("Transaction error!");
                }
                else
                {

                    //For the benefit of the bank, first implement the withdraw function to get money from the account
                    bool resultForWithdraw = Withdraw(amount, chargesFee, destinationAccount.AccountNumber, command, "T");

                    //If an exception occurs in the withdraw process,
                    //then the exception is caught directly and the transfer is aborted.
                    if (resultForWithdraw == false)
                    {
                       Console.WriteLine($"Transfer failed in withdraw processing.");
                    }
                    else
                    {
                       //If the withdrawal is successful,
                       //the deposit function is executed to deposit the money into the target account.
                       bool resultForDeposit = destinationAccount.Deposit(amount, 0, command, "T");


                       //If any anomaly occurs during the deposit, the transfer process is also aborted
                       if (resultForDeposit == true)
                       {
                          Console.WriteLine($"whole transaction process is successful.  " + "$Amount is " + amount);
                       }
                       else
                       {
                          Console.WriteLine($"Transfer failed in deposit processing.");
                       }
                    }
                }
            }
            catch (TransactionException te)
            {
                Console.WriteLine(te.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



        /*
         * Used to print Transaction history on the console.
         */
        public void DisplayStatement()
        {

            var table = new ConsoleTable("Transation ID", "Transaction Type", "From Account",
                "Destination Acount", "Amount", "Comment", "Issuse Date");

            //sort the Transaction List so that the most recent transaction is placed first.
            Transactions.Sort((a,b) => b.TransactionTimeUtc.CompareTo(a.TransactionTimeUtc));


            for (int i = 1; i <= Transactions.Count; i++)
            {

                //If the transaction is completely printed out,
                //the table is displayed directly and the user returns to the Min Menu
                if (i == Transactions.Count)
                {
                    table.AddRow(Transactions[i-1].TransactionID, Transactions[i-1].TransactionType,
                                 Transactions[i-1].AccountNumber,
                                 Transactions[i-1].DestinationAccountNumber, Transactions[i-1].Amount,
                                 Transactions[i-1].Comment, Transactions[i-1].TransactionTimeUtc.ToLocalTime());

                    table.Write();
                    Console.WriteLine();
                    Console.WriteLine($"The above is the full transfer record.");
                    Console.WriteLine($"Press any key to return to main menu.");
                    Console.ReadKey();
                    Console.Clear();
                }

                //If the current index is divisible by 5, then four transations have been recorded.
                else if (i % 5 == 0)
                {
                    
                    table.Write();
                    Console.WriteLine();
                    Console.WriteLine("press any key to next page");
                    Console.ReadKey();
                    Console.Clear();

                    //Empty current table
                    table = new ConsoleTable("Transation ID", "Transaction Type", "From Account",
                    "Destination Acount", "Amount", "Comment", "Issuse Date");


                    //Add the current transaction to the new table.
                    table.AddRow(Transactions[i-1].TransactionID, Transactions[i-1].TransactionType,
                                 Transactions[i-1].AccountNumber,
                                 Transactions[i-1].DestinationAccountNumber, Transactions[i-1].Amount,
                                 Transactions[i-1].Comment, Transactions[i-1].TransactionTimeUtc.ToLocalTime());

                }
                else
                {
                    //If none of these conditions are met, then the loop adds a row to the current table.
                    table.AddRow(Transactions[i-1].TransactionID, Transactions[i-1].TransactionType,
                                 Transactions[i-1].AccountNumber,
                                 Transactions[i-1].DestinationAccountNumber, Transactions[i-1].Amount,
                                 Transactions[i-1].Comment, Transactions[i-1].TransactionTimeUtc.ToLocalTime());
                }
            }
        }
    } 
}
