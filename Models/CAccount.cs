using System;
using System.Collections.Generic;
using wdt_Assignment1_s3757573.ExceptionBox;
using Microsoft.Extensions.Configuration;
using wdt_Assignment1_s3757573.Managers;
using System.Threading.Tasks;


/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Create the Checking Account Object.
 */

namespace wdt_Assignment1_s3757573
{
    public class CAccount : Account
    {
        public decimal CminBalance { get; }
       
        public CAccount(){}

        public CAccount(int AccountNumber, string AccountType, int CustomerID, decimal Balance, List<Transactions> Transactions)
           : base(AccountNumber, AccountType, CustomerID, Balance, Transactions)
        {
            CminBalance = 200;
            
        }


       /*
        * @premeter amount(How much money the user wants to withdraw).
        * @premeter destinationAccount(This parameter is defined as 0 in the deposit method).
        * @premeter command(hard code for withdraw).
        * @premeter transactionType(hard code for "W").
        * @premeter ChargesFee(0.1).
        * 
        * Withdraw from Checking Account.
        */
        override public bool Withdraw(decimal amount, decimal chargesFee, int DistinationAccount, string command, string transactionType)
        {
           
            try
           {
                //Initializes the total withdrawal amount
                decimal totalAmount = new decimal(0);
                var dateTime = DateTime.Now;

                if (Transactions.Count > 4)
                {
                    //The total amount should be plus the amount of charge.
                    totalAmount = chargesFee + amount;
                    string commondForCharges = "Charges Fee for Transaction Server.";


                    //Determine whether the total amount to be withdrawn from the current account exceeds Balance.
                    if (totalAmount <= Balance && Balance - totalAmount > CminBalance && amount > 0)
                    {
                        Balance -= totalAmount;

                        Console.WriteLine(amount + $" is withdraw successful");
                        var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
                        var connectionKey = configuration["ConnectionString"];
                        AccountManager accountManager = new AccountManager(connectionKey);
                        TransactionManager transactionManager = new TransactionManager(connectionKey);

                        //Add new ChargeFee asynchronously
                        Task updateAccount = accountManager.UpdateAmount(Balance, AccountNumber);
                        Task addTable = transactionManager.AddTransactionTable(new Transactions(AccountNumber, DistinationAccount, command, amount, transactionType, dateTime));
                        Task addTable1 = transactionManager.AddTransactionTable(new Transactions(AccountNumber, 0, commondForCharges, chargesFee, transactionType, dateTime));
                        return true;

                    }
                    else
                    {
                        throw new AccountBalanceException($"Negetive and empty amount is not allowed.");
                    }
                }
                else
                {
                    //without charges Fee.
                    totalAmount = amount;

                    if (totalAmount <= Balance && Balance - totalAmount > CminBalance && amount > 0)
                    {
                        Balance -= totalAmount;

                        Console.WriteLine(amount + $" is withdraw successful");
                        var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
                        var connectionKey = configuration["ConnectionString"];
                        AccountManager accountManager = new AccountManager(connectionKey);
                        TransactionManager transactionManager = new TransactionManager(connectionKey);
                        Task updateAccount = accountManager.UpdateAmount(Balance, AccountNumber);
                        Task addTable = transactionManager.AddTransactionTable(new Transactions(AccountNumber, DistinationAccount, command, amount, transactionType, dateTime));
                        return true;

                    }
                    else
                    {
                        throw new AccountBalanceException($"Negetive and empty amount is not allowed.");
                    }
                }

            }
            catch (AccountBalanceException ae)
            {
               Console.WriteLine(ae.Message);
               return false;
            }
            catch(Exception e)
            {
               Console.WriteLine(e.Message);
               return false;
            }
        }
    }
}
