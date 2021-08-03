
/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Use to create an Account factory that instantiates an Object 
 * when an Acount needs to be read from the database
 */

namespace wdt_Assignment1_s3757573
{
    public class AccountFactory
    {
        public Account Get(string AccountType)
        {
            if (AccountType.CompareTo("C") == 0)
            {
                return new CAccount();
            }
            else
            {
                return new SAccount();
            }
        }
    }
}
