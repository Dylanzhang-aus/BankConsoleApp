using System.ComponentModel.DataAnnotations;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Create the Login Object.
 */

namespace wdt_Assignment1_s3757573.Models
{

    public class Login
    {
        [Range(8,8),Required]
        public string LoginID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required, StringLength(64)]
        public string PasswordHash { get; set; }

        public Login() { }

        public Login(string LoginID, string PasswordHash,int CustomerID)
        {
            this.LoginID = LoginID;
            this.CustomerID = CustomerID;
            this.PasswordHash = PasswordHash;
        }
    }
}
