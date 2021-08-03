using System;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Capture the out-of-bounds number that appears 
 * when the user makes a menu selection.
 */


namespace wdt_Assignment1_s3757573.ExceptionBox
{
    public class MenuSelectException : ApplicationException
    {
        public MenuSelectException(string message) : base() { }

        public override string Message => base.Message;
    }
}
