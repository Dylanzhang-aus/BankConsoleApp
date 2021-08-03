using System;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * This class is designed to catch invalid account number exceptions 
 * that will be thrown if the user tries to transfer money to a non-existent account.
 */

namespace wdt_Assignment1_s3757573.ExceptionBox
{
    public class AccountNoFundException : ApplicationException
    {
        public AccountNoFundException(string message) : base() { }

        public override string Message => "The account number no fund, Please enter again.";

    }
}
