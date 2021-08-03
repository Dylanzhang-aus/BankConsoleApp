using System;
using System.Collections.Generic;
using MyLibrary;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Used to read customer table and manage customer objdect in a database.
 */

namespace wdt_Assignment1_s3757573.Managers
{
    public class CustomerManager
    {
        private readonly string ConnectionKey;
        public List<Customer> CustomerList { get; }


        /*
         * Initialize the Customer class and all related classes.
         */
        public CustomerManager(string connectionKey)
        {
            ConnectionKey = connectionKey;

            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            var AccountManager = new AccountManager(connectionKey);
            var LoginManager = new LoginManager(connectionKey);
            var command = connection.CreateCommand();
            command.CommandText = "select * from Customer";


            //Reads the data from the database and generates a Customer Object.
            CustomerList = UsefulFunction.GetTable(command).Select().Select(x => new Customer
            {
                CustomerID = (int)x["CustomerID"],
                Name = (string)x["Name"],
                Address = (string)x["Address"],
                City = (string)x["City"],
                PostCode = (string)x["PostCode"],
                Accounts = AccountManager.GetAccounts((int)x["CustomerID"]),
                Login = LoginManager.GetLoginByCustomerID((int)x["CustomerID"])

            }).ToList();
        }



        /*
         * Insert new customer into customer table.
         */
        public void AddCustomerTable(Customer customer)
        {
            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            connection.Open();

            //Determines whether a nullable property is NULL, and if so, initializes it with "NULL".
            if (customer.City == null)
            {
               customer.City = "NULL";
            }
            if(customer.Address == null)
            {
               customer.Address = "NULL";
            }
            if(customer.PostCode == null)
            {
               customer.PostCode = "NULL";
            }


            var command = connection.CreateCommand();
            command.CommandText = "insert into Customer(CustomerID,Name,Address,City,PostCode) values (@CustomerID, @Name, @Address, @City, @PostCode)";
            command.Parameters.AddWithValue("CustomerID", customer.CustomerID);
            command.Parameters.AddWithValue("Name", customer.Name);
            command.Parameters.AddWithValue("PostCode", customer.PostCode);
            command.Parameters.AddWithValue("City", customer.City);
            command.Parameters.AddWithValue("Address", customer.Address);
            
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
    }

}
