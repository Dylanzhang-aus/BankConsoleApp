using wdt_Assignment1_s3757573.WebServer;
using Microsoft.Extensions.Configuration;
using System;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * This class is the entry point to the program.
 */

namespace wdt_Assignment1_s3757573
{
    class Runner
    {

        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("ConnectionKey.json").Build();
            var connectionKey = configuration["ConnectionString"];
            LoadWebData.LoadingData(connectionKey);
            Console.WriteLine("Loading data successfully");
            Console.WriteLine();
            new LoginMenu(connectionKey).LoginMenuRun();
        }
    }
}
