using BankApp.Classes;
using BankApp.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BankApp.Methods
{
    public class Transactions
    {
       
       // public float accountBalance;
        public void WithdrawMoney()
        {
            BankApp_DbContext db = new BankApp_DbContext();
            List<User> users = db.GetAllEntities<User>();
            List<Account> accounts = db.GetAllEntities<Account>();

            Console.WriteLine("Enter your identifier (firstname for now):");
            string identifier = Console.ReadLine();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            identifier = textInfo.ToTitleCase(identifier);

            // Find the user with the specified first name
            User foundUser = users.FirstOrDefault(user => user.FirstName.Equals(identifier, StringComparison.OrdinalIgnoreCase));

            // Check if the user was found
            if (foundUser != null)
            {
                // Prompt for the amount to withdraw
                Console.WriteLine("Enter amount to withdraw:");
                if (int.TryParse(Console.ReadLine(), out int withdrawAmount))
                {
                    // Find the account associated with the found user
                    Account foundAccount = accounts.FirstOrDefault(account => account.userId.Equals(foundUser.Id));

                    // Check if the account was found
                    if (foundAccount != null)
                    {
                        // Check if there are sufficient funds in the account
                        if (withdrawAmount > foundAccount.accountBalance)
                        {
                            Console.WriteLine("Insufficient Funds");
                        }
                        else
                        {
                            // Perform the withdrawal
                            foundAccount.accountBalance -= withdrawAmount;
                            db.UpdateEntities(accounts); // Update database
                            TransactionHistory RecordedTransaction = new TransactionHistory(foundAccount.userId, "Debit", withdrawAmount);
                            db.AddEntity(RecordedTransaction);
                            Console.WriteLine($"Success !! You have withdrawn {withdrawAmount} and your new balance is {foundAccount.accountBalance} ");
                            Console.WriteLine("Press Enter key to return to main menu");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sorry, you do not have an account with us.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid amount entered.");
                }
            }
            else
            {
                Console.WriteLine("User not found. Please register first.");
            }
        }

        public void DepositMoney()
        {
            BankApp_DbContext db = new BankApp_DbContext();
            List<User> users = db.GetAllEntities<User>();
            List<Account> accounts = db.GetAllEntities<Account>();

            Console.WriteLine("Enter your identifier (firstname for now):");
            string identifier = Console.ReadLine();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            identifier = textInfo.ToTitleCase(identifier);

            // Find the user with the specified first name
            User foundUser = users.FirstOrDefault(user => user.FirstName.Equals(identifier, StringComparison.OrdinalIgnoreCase));

            // Check if the user was found
            if (foundUser != null)
            {
                Console.WriteLine("Enter amount to deposit:");
                if (int.TryParse(Console.ReadLine(), out int depositAmount))
                {
                    // Find the account associated with the found user
                    Account foundAccount = accounts.FirstOrDefault(account => account.userId.Equals(foundUser.Id));

                    // Check if the account was found
                    if (foundAccount != null)
                    {
                        // Perform the deposit
                        foundAccount.accountBalance += depositAmount;
                        db.UpdateEntities(accounts); // Update database
                        TransactionHistory RecordedTransaction = new TransactionHistory(foundAccount.userId,"Credit",depositAmount);
                        db.AddEntity(RecordedTransaction);
                        Console.WriteLine($"Success!! You have deposited {depositAmount} and your new balance is {foundAccount.accountBalance}.");
                        Console.WriteLine("Press Enter key to return to main menu");
                    }
                    else
                    {
                        Console.WriteLine("Sorry, you do not have an account with us.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid amount entered.");
                }
            }
            else
            {
                Console.WriteLine("User not found. Please register first.");
            }
        }


        public void Transfer(float amount)
        {
            BankApp_DbContext db = new BankApp_DbContext();
            List<User> users = db.GetAllEntities<User>();
            List<Account> accounts = db.GetAllEntities<Account>();
            Guid senderUserId = Guid.Empty;
            Guid receiverUserId = Guid.Empty;

            Console.WriteLine("Enter sender identifier (first name):");
            string senderIdentifier = Console.ReadLine();
            Console.WriteLine("Enter receiver identifier (first name):");
            string receiverIdentifier = Console.ReadLine();

            foreach (User user in users)
            {
                Console.WriteLine($"User: {user.FirstName}, ID: {user.Id}");
            }

            // Find sender and receiver user IDs (case-insensitive comparison)
            foreach (User user in users)
            {
                if (user.FirstName.Equals(senderIdentifier, StringComparison.OrdinalIgnoreCase))
                {
                    senderUserId = user.Id;
                }
                if (user.FirstName.Equals(receiverIdentifier, StringComparison.OrdinalIgnoreCase))
                {
                    receiverUserId = user.Id;
                }
            }

            // Check if both users were found
            if (senderUserId == Guid.Empty || receiverUserId == Guid.Empty)
            {
                Console.WriteLine($"One or both users not found. Please make sure both sender and receiver are registered. Sender ID: {senderUserId}, Receiver ID: {receiverUserId}");
                return;
            }

            Account senderAccount = null;
            Account receiverAccount = null;

            // Find sender and receiver accounts
            foreach (Account account in accounts)
            {
                if (account.userId.Equals(senderUserId))
                {
                    senderAccount = account;
                }
                if (account.userId.Equals(receiverUserId))
                {
                    receiverAccount = account;
                }
            }

            // Check if both accounts were found
            if (senderAccount == null || receiverAccount == null)
            {
                Console.WriteLine("One or both accounts not found. Please make sure both sender and receiver have accounts.");
                return;
            }

            // Check if sender has sufficient funds
            if (senderAccount.accountBalance < amount)
            {
                Console.WriteLine("Insufficient funds in the sender's account.");
                return;
            }

            // Perform the transfer
            senderAccount.accountBalance -= amount;
            receiverAccount.accountBalance += amount;

            // update changes to database
            db.UpdateEntities(accounts);

            TransactionHistory senderTransaction = new TransactionHistory(senderUserId, "Transfer Out", -amount);
            TransactionHistory receiverTransaction = new TransactionHistory(receiverUserId, "Transfer In", amount);
            db.AddEntity(senderTransaction);
            db.AddEntity(receiverTransaction);

            Console.WriteLine($"Transfer successful! {amount} has been transferred from {senderIdentifier} to {receiverIdentifier}.");
            Console.WriteLine($"Sender's new balance: {senderAccount.accountBalance}");
            Console.WriteLine($"Receiver's new balance: {receiverAccount.accountBalance}");
        }
    }
}
