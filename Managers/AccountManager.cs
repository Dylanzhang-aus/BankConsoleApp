using System;
using System.Collections.Generic;
using MyLibrary;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Used to read account table and manage account objdect in a database.
 */

namespace wdt_Assignment1_s3757573.Managers
{
    public class AccountManager
    {
        private readonly string ConnectionKey;

        public AccountManager(string connectionKey)
        {
            ConnectionKey = connectionKey;
        }


        public List<Account> GetAccounts(int CustomerID)
        {

            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            var command = connection.CreateCommand();
            command.CommandText = "select * from Account where CustomerID = @CustomerID";
            command.Parameters.AddWithValue("CustomerID", CustomerID);
            var TransactionManager = new TransactionManager(ConnectionKey);


            //Generate a new Account Object dynamically using the factory pattern.
            AccountFactory accountFactory = new AccountFactory();


            //reading data from current Account table in database.
            var accountList = UsefulFunction.GetTable(command).Select().Select(x =>
            {

                //Determine which Account needs to be generated and initialse account object.
                var account = accountFactory.Get((string)x["AccountType"]);
               
                account.AccountNumber = (int)x["AccountNumber"];
                account.AccountType = (string)x["AccountType"];
                account.CustomerID = (int)x["CustomerID"];
                account.Balance = (decimal)x["Balance"];
                account.Transactions = TransactionManager.GetTransactions((int)x["AccountNumber"]);

                return account;
            
            }).ToList();

            return accountList;
        }


        public void AddAccountTable(Account account, Customer customer)
        {

            //open database by using the function in class libary.
            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            connection.Open();


            //inserts the new Object to the Table based on the foreign key in the existing Customer class.
            var command = connection.CreateCommand();
            command.CommandText = "insert into Account(AccountNumber,AccountType,CustomerID,Balance) values (@AccountNumber, @AccountType, @CustomerID, @Balance)";
            command.Parameters.AddWithValue("AccountNumber", account.AccountNumber);
            command.Parameters.AddWithValue("AccountType", account.AccountType);
            command.Parameters.AddWithValue("CustomerID", customer.CustomerID);
            command.Parameters.AddWithValue("Balance", account.Balance);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                UsefulFunction.CloseConnection(connection);
            }
        }


        /*
         * The purpose of this method is to determine whether the destination Account entered 
         * by the user exists in the existing database based on the accountNumber entered by the user.
         */
        public List<Account> GetAccountsByAccountNumber(int AccountNumber)
        {

            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            var command = connection.CreateCommand();
            command.CommandText = "select * from Account where AccountNumber = @AccountNumber";
            command.Parameters.AddWithValue("AccountNumber", AccountNumber);

            var TransactionManager = new TransactionManager(ConnectionKey);
            AccountFactory accountFactory = new AccountFactory();

            var accountList = UsefulFunction.GetTable(command).Select().Select(x =>
            {
                var account = accountFactory.Get((string)x["AccountType"]);

                account.AccountNumber = (int)x["AccountNumber"];
                account.AccountType = (string)x["AccountType"];
                account.CustomerID = (int)x["CustomerID"];
                account.Balance = (decimal)x["Balance"];
                account.Transactions = TransactionManager.GetTransactions((int)x["AccountNumber"]);

                return account;
            
            }).ToList();

            return accountList;
        }



        /*
         * This method is called to update Balance in the database each time the user acts on the account
         */
        public async Task UpdateAmount(decimal Balance,int AccountNumber)
        {
            await Task.Run(() =>
            {
                using var connection = UsefulFunction.DbConnection(ConnectionKey);
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "update Account set Balance = @Balance where AccountNumber = @AccountNumber";
                command.Parameters.AddWithValue("Balance", Balance);
                command.Parameters.AddWithValue("AccountNumber", AccountNumber);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    Console.WriteLine(se.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    UsefulFunction.CloseConnection(connection);
                }
            });
        }
    }
}
