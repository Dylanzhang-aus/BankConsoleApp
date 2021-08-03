using System;
using wdt_Assignment1_s3757573.Models;
using System.Collections.Generic;
using MyLibrary;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Reading login table and manager it.
 */

namespace wdt_Assignment1_s3757573.Managers
{
    public class LoginManager
    {
        private readonly string ConnectionKey;


        public LoginManager(string ConnectionKey)
        {
            this.ConnectionKey = ConnectionKey;
        }


        /*
         * reading the login table, initialse the login object
         */
        public List<Login> GetLoginByCustomerID(int CustomerID)
        {
            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            var command = connection.CreateCommand();
            command.CommandText = "select * from Login where CustomerID = @CustomerID";
            command.Parameters.AddWithValue("CustomerID", CustomerID);
            var login = new List<Login>();
            try
            {
                login = UsefulFunction.GetTable(command).Select().Select(x => new Login
                {
                    LoginID = (string)x["LoginID"],
                    CustomerID = (int)x["CustomerID"],
                    PasswordHash = (string)x["PasswordHash"]
                }).ToList();
                
            }
            catch(SqlException se)
            {
                Console.WriteLine(se);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                UsefulFunction.CloseConnection(connection);
            }

            return login;
        }



        /*
         * Add a new Login Project to the Login Table.
         */
        public void AddLoginTable(Login login, Customer customer)
        {

            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "insert into Login(LoginID, CustomerID, PasswordHash) values (@LoginID, @CustomerID, @PasswordHash)";
            command.Parameters.AddWithValue("LoginID", login.LoginID);
            command.Parameters.AddWithValue("CustomerID", customer.CustomerID);
            command.Parameters.AddWithValue("PasswordHash", login.PasswordHash);

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
         * When the user logs in with the loginID, 
         * the corresponding customer information is found by reading the login table.
         */
        public List<Login> GetLoginByLoginID(string LoginID)
        {
            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            var command = connection.CreateCommand();
            command.CommandText = "select * from Login where LoginID = @LoginID";
            command.Parameters.AddWithValue("LoginID", LoginID);
            var login = new List<Login>();

            try
            {
                login = UsefulFunction.GetTable(command).Select().Select(x => new Login
                {
                    LoginID = (string)x["LoginID"],
                    CustomerID = (int)x["CustomerID"],
                    PasswordHash = (string)x["PasswordHash"]
                }).ToList();
            }

            catch(SqlException se)
            {
                Console.WriteLine(se);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                UsefulFunction.CloseConnection(connection);
            }

            return login;
        }

    }
}
