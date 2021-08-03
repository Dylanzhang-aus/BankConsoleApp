using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using wdt_Assignment1_s3757573.Models;

/*
 * @author Hanyuan Zhang - s3757573, RMIT 2021
 * 
 * Create the Customer Object.
 */

namespace wdt_Assignment1_s3757573
{
    public class Customer
    {
        [Required, Range(4,4)]
        public int CustomerID { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(4)]
        public string PostCode { get; set; }
        public List<Login> Login { get; set; }
        public List<Account> Accounts { get; set; }


        public Customer(){}


        public Customer(int CustomerID, string Name, string Address, string City, string PostCode)
        {
            this.CustomerID = CustomerID;
            this.Name = Name;
            this.Address = Address;
            this.City = City;
            this.PostCode = PostCode;
            Login = new List<Login>();
            Accounts = new List<Account>();
        }
    }
}
