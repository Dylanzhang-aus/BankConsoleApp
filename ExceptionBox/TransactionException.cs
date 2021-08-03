using System;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Catch all exceptions that caused the transfer to fail when the user made the transfer.
 */

namespace wdt_Assignment1_s3757573.ExceptionBox
{
    public class TransactionException : ApplicationException
    {
        public TransactionException(string message) : base() { }

        public override string Message => "$Your are trying transfer into same Account which is not allowed";
    }
}
