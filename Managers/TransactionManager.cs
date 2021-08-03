using System;
using System.Linq;
using System.Collections.Generic;
using MyLibrary;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Reading Transaction table and manager it.
 */

namespace wdt_Assignment1_s3757573.Managers
{
    public class TransactionManager
    {
        private readonly string ConnectionKey;

        public TransactionManager(string connectionKey)
        {
            ConnectionKey = connectionKey;
        }


        /*
         * The purpose of this method is to call this method to initialize 
         * the Transaction of the Customer when that Customer is initialized by accountNumber FK.
         */
        public List<Transactions> GetTransactions(int AccountNumber)
        {
            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "select * from [Transaction] where AccountNumber = @AccountNumber";
            command.Parameters.AddWithValue("AccountNumber", AccountNumber);

            var TransactionList = UsefulFunction.GetTable(command).Select().Select(x => new Transactions
            {
                TransactionID = (int) x["TransactionID"],
                TransactionType = (string)x["TransactionType"],
                AccountNumber = (int)x["AccountNumber"],
                DestinationAccountNumber = x["DestinationAccountNumber"] as int?,
                Amount = (decimal)x["Amount"],
                Comment = (string)x["Comment"],
                TransactionTimeUtc = (DateTime) x["TransactionTimeUtc"]
            }).ToList();

            return TransactionList;
        }



        /*
         * When a new transaction is generated, add the transaction to the table in database.
         */
        public async Task AddTransactionTable(Transactions transaction)
        {
            await Task.Run(() =>
            {
                using var connection = UsefulFunction.DbConnection(ConnectionKey);

                if (transaction.Comment == null)
                {
                    transaction.Comment = "NULL";
                }

                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "insert into [Transaction] (TransactionType,AccountNumber,DestinationAccountNumber,Amount,Comment,TransactionTimeUtc) values (@TransactionType, @AccountNumber, @DestinationAccountNumber, @Amount, @Comment, @TransactionTimeUtc)";
                command.Parameters.AddWithValue("TransactionType", transaction.TransactionType);
                command.Parameters.AddWithValue("AccountNumber", transaction.AccountNumber);


                //For deposit, withdraw, and destination accounts, the destination Number is marked empty
                if (transaction.DestinationAccountNumber == null || transaction.DestinationAccountNumber == 0)
                {
                    command.Parameters.AddWithValue("DestinationAccountNumber", value: DBNull.Value);

                }
                else
                {
                    command.Parameters.AddWithValue("DestinationAccountNumber", transaction.DestinationAccountNumber);
                }

                command.Parameters.AddWithValue("Amount", transaction.Amount);
                command.Parameters.AddWithValue("Comment", transaction.Comment);
                command.Parameters.AddWithValue("TransactionTimeUtc", transaction.TransactionTimeUtc);

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
