using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankApp.Classes;
using BankApp.Database;

namespace BankApp.Methods
{
    public  class UserMethods
    {

        public void Register(string firstName, string lastName, string email, string password)
        {
            var id = Guid.NewGuid();
            //var validEmail = new Email();

            User newUser = new User(id, firstName, lastName, email, password);
            // BankDataBase_liteDb.UserDatabase.Add(newUser);
            
            BankApp_DbContext db = new BankApp_DbContext();
            db.AddEntity(newUser);
        }
    }
}

//        public void DisplayAccountInformation()
//        {

//            Console.WriteLine("| FULL NAME | ACCOUNT NUMBER | ACCOUNT TYPE | AMOUNT BAL | NOTE |");
//            Console.WriteLine("|-----------|----------------|--------------|------------|------|");

//            List<User> customers = BankDataBase_liteDb.UserDatabase;
//            foreach (var customer in customers)
//            {
//                foreach (var account in customer.GetAccounts())
//                {
//                    Console.WriteLine($"| {customer.FullName} | {account.AccountNumber} | {account.GetType().Name} | {account.Balance} | {account.Note} |");
//                }
//            }
//        }



//    }
//}
