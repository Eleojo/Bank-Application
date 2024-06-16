using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Classes
{
    public class User
    {
        //public User(Guid accountBVN, string firstname, string lastname, string phoneNumber, string email)
        //{
        //    //this.accountBVN = accountBVN;
        //    this.phoneNumber = phoneNumber;
        //    this.firstname = firstname;
        //    this.lastname = lastname;
        //    //this.accountPassword = accountPassword;
        //    this.email = email;
        //    //this.accountNumber = accountNumber;
        //    //this.accountType = accountType;
        //}

        //public Guid Id { get; set; }
        //public string phoneNumber { get; set; }
        //public string firstname { get; set; }
        //public string lastname { get; set; }
        ////public string accountPassword { get; set; }
        //public string email { get; set; }

        //private List<Account> accounts = new List<Account>();
        // public string accountNumber { get; set; }
        //public string accountType { get; set; }


        public User(Guid id, string firstName, string lastName, string email, string password)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }



    }
}
