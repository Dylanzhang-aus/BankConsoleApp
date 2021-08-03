using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using wdt_Assignment1_s3757573.Managers;
using wdt_Assignment1_s3757573.Models;
using System.Data.SqlClient;
using System;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * The class is to loading the data from web server.
 */

namespace wdt_Assignment1_s3757573.WebServer
{
    public class LoadWebData
    {


        /*
         * start to loading data.
         * because this is no dependency in this method, 
         * so write at static method.
         */
        public static void LoadingData(string connectionKey)
        {

            //Initialize all managers needed.
            var customerManager = new CustomerManager(connectionKey);
            var loginManager = new LoginManager(connectionKey);
            var accountManager = new AccountManager(connectionKey);
            var transactionManager = new TransactionManager(connectionKey);

            //If an object is found in the table, exit loding directly
            if (customerManager.CustomerList.Count > 0)
            {
                return;
            }
            else
            {
                DbManager.CreateTables(connectionKey);
                using var webClient = new HttpClient();
                try
                {
                    var urlCusAndAccount = "https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/";
                    var urlLogin = "https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/";
                    var jsonCusAndAccount = webClient.GetStringAsync(urlCusAndAccount).Result;
                    var jsonLogin = webClient.GetStringAsync(urlLogin).Result;

                    var customer = JsonConvert.DeserializeObject<List<Customer>>(jsonCusAndAccount, new JsonSerializerSettings
                    {
                        DateFormatString = "dd/MM/yyyy hh:mm:ss tt",
                    });
                    var Login = JsonConvert.DeserializeObject<List<Login>>(jsonLogin);

                    foreach (var c in customer)
                    {
                        customerManager.AddCustomerTable(c);

                        //I chose to load Customer Web as well as Login Web.
                        foreach (var l in Login)
                        {
                            if (l.CustomerID.Equals(c.CustomerID))
                            {
                                loginManager.AddLoginTable(l, c);
                            }
                        }

                        //Loading account table. 
                        foreach (var a in c.Accounts)
                        {
                            accountManager.AddAccountTable(a, c);

                            //loading transation table.
                            foreach (var t in a.Transactions)
                            {
                                t.AccountNumber = a.AccountNumber;
                                t.Amount = a.Balance;
                                t.TransactionType = "D";
                                transactionManager.AddTransactionTable(t);
                            }
                        }
                    }
                }
                catch(SqlException se)
                {
                    Console.WriteLine(se.Message); 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
