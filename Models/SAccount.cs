using System;
using System.Collections.Generic;
using wdt_Assignment1_s3757573.ExceptionBox;
using wdt_Assignment1_s3757573.Managers;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Create the Saving Account Object. The logic in this class is same as CAccount class
 * so, there is no more commend
 */

namespace wdt_Assignment1_s3757573
{
    public class SAccount : Account
    {

        public decimal SminBalance { get;}
       

        public SAccount(){}

        public SAccount(int AccountNumber, string AccountType, int CustomerID, decimal Balance, List<Transactions> Transactions)
            : base(AccountNumber, AccountType, CustomerID, Balance,Transactions)
        {
            SminBalance = 0;     
        }
            

        override public bool Withdraw(decimal amount, decimal chargesFee, int destinationAccount, string command, string transactionType)
        {
            try
            {
                decimal totalAmount = new decimal(0);
                var dateTime = DateTime.Now;


                if (Transactions.Count > 4)
                {
                    totalAmount = chargesFee + amount;
                    string commondForCharges = "Charges Fee for Transaction Server.";

                    if (totalAmount <= Balance && amount > 0)
                    {
                        Balance -= totalAmount;

                        Console.WriteLine(amount + $" is withdraw successful");
                        var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
                        var connectionKey = configuration["ConnectionString"];
                        AccountManager accountManager = new AccountManager(connectionKey);
                        TransactionManager transactionManager = new TransactionManager(connectionKey);
                        Task updateAccount = accountManager.UpdateAmount(Balance, AccountNumber);
                        Task addTable = transactionManager.AddTransactionTable(new Transactions(AccountNumber, destinationAccount, command, amount, transactionType, dateTime));
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
                    totalAmount = amount;
                    if (totalAmount <= Balance && amount > 0)
                    {
                        Balance -= totalAmount;

                        Console.WriteLine(amount + $" is withdraw successful");
                        var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
                        var connectionKey = configuration["ConnectionString"];
                        AccountManager accountManager = new AccountManager(connectionKey);
                        TransactionManager transactionManager = new TransactionManager(connectionKey);
                        Task updateAccount = accountManager.UpdateAmount(Balance, AccountNumber);
                        Task addTable = transactionManager.AddTransactionTable(new Transactions(AccountNumber, destinationAccount, command, amount, transactionType, dateTime));
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
