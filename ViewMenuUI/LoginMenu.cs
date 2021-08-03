using wdt_Assignment1_s3757573.Managers;
using System;
using SimpleHashing;
using MyLibrary;
using wdt_Assignment1_s3757573.ViewMenuUI;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * The class handle all LoginMenu.
 */

namespace wdt_Assignment1_s3757573
{
    public class LoginMenu
    {
        private readonly CustomerManager customerManager;
        private readonly LoginManager loginManager;


        public LoginMenu(string ConnectionKey)
        {
            customerManager = new CustomerManager(ConnectionKey);
            loginManager = new LoginManager(ConnectionKey);
          
        }


        /*
         * To display the Login Menu.
         */
        public void LoginMenuRun()
        {
            Console.WriteLine($"      Welcome to MCBA Application !     ");
            Console.WriteLine();
            Console.WriteLine($"============== Login Menu ================");

            while (true)
            {
                Console.WriteLine($"Please enter your Login ID :");
                var InputLoginID = Console.ReadLine();
                Console.WriteLine();


                //Check the login table for the existence of the loginID entered by the user.
                var loginList = loginManager.GetLoginByLoginID(InputLoginID);


                //if return a empty loginList which means there is no LoginID which user input.
                if(loginList.Count == 0)
                {
                    Console.WriteLine($"Input LoginID inexistent, Please check your Input.");
                    continue;
                }
                else
                {
                    while (true)
                    {
                        //Since the loginID is unique in the table,
                        //only one object can be in the returned list, so call this object directly with index.
                        string password = loginList[0].PasswordHash;
                        Console.WriteLine($"Please enter your PassWord :");

                        //using the method in class libary to make mask for user input.
                        string InputPassword = UsefulFunction.InputmaskMask();
                        Console.WriteLine();
                        
                        if(InputPassword == null)
                        {
                            Console.WriteLine($"You can not enter empty, please enter again!");
                            Console.WriteLine();
                            continue;
                        }
                        else
                        {
                            //using PBKDF to decode user input.
                            if (PBKDF2.Verify(password, InputPassword) == false)
                            {
                                Console.WriteLine($"Incorrect password, please check and enter again!");
                                Console.WriteLine();
                                continue;
                            }
                            else
                            {
                               //When the password matches successfully,
                               //the user finds the corresponding customer using the CusomerID,
                               //and passes the customer to the Main menu.
                               foreach ( var c in customerManager.CustomerList)
                               {
                                 if (c.CustomerID.Equals(loginList[0].CustomerID))
                                 {
                                    Console.Clear();
                                    MainMenu mainMenu = new MainMenu(c);
                                    mainMenu.MainMenuRun();  
                                 }   
                               }
                                  break;
                            } 
                        }  
                    }
                    break;
                }
            }
        }
    }
}
