using System.IO;
using MyLibrary;
using System;
using System.Data.SqlClient;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Initialize the database and table.
 */

namespace wdt_Assignment1_s3757573.Managers
{
    public class DbManager
    {

        public static void CreateTables(string ConnectionKey)
        {
            using var connection = UsefulFunction.DbConnection(ConnectionKey);
            connection.Open();
            var command = connection.CreateCommand();

            //The verbose path is hidden. The actual path is "bin/Debug/net5.0/CreateTable.sql".
            command.CommandText = File.ReadAllText("CreateTables.sql");

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
