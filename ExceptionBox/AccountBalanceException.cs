using System;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * This class is designed to catch user input that causes account amounts to be unusual, 
 * such as negative numbers and zeros
 */

namespace wdt_Assignment1_s3757573.ExceptionBox
{
    

    public class AccountBalanceException : ApplicationException
    {
        public AccountBalanceException(string message) : base(){}

        public override string Message => "$Your input caused an exception to the current account. Please check your amount.";
    }
}
