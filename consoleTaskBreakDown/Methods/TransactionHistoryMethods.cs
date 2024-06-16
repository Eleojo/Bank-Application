using BankApp.Classes;
using BankApp.Database;
using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Methods
{
    internal class TransactionHistoryMethods
    {
        public void DisplayTransactionHistory(Guid userId)
        {
            BankApp_DbContext db = new BankApp_DbContext();
            List<TransactionHistory> transactions = db.GetUserTransactions(userId);

            if (transactions.Count == 0)
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            ConsoleTableBuilder
                .From(transactions.Select(t => new
                {
                    t.Id,
                    t.TransactionType,
                    t.Amount,
                    t.Timestamp
                }).ToList())
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
        }


        public void ViewTransactionHistory()
        {
            BankApp_DbContext db = new BankApp_DbContext();
            List<User> users = db.GetAllEntities<User>();
            Console.WriteLine("Enter your identifier (firstname for now):");

            string identifier = Console.ReadLine();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            identifier = textInfo.ToTitleCase(identifier);

            foreach (User user in users)
            {
                if (user.FirstName.Equals(identifier))
                {
                    DisplayTransactionHistory(user.Id);
                    return;
                }
            }

            Console.WriteLine("User not found. Please register first.");
        }


    }
}

