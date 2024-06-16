using BankApp.Classes;
using BankApp.Database;
using ConsoleTableExt;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Methods
{
    internal class AccountMethods
    {

        public void UpdateMyAccountProfile()
        {

        }


        public static void RegisterAndOpenAccount()
        {
            BankApp_DbContext db = new BankApp_DbContext();

            string email;
            string firstName;
            string lastName;
            string password;

            bool validEmail;
            bool validName;
            bool validPassword;

            // Prompt for and validate first name
            do
            {
                Console.WriteLine("Enter your FirstName:");
                firstName = Console.ReadLine();
                validName = Validations.IsValidName(firstName);
                if (!validName)
                {
                    Console.WriteLine("Name cannot be empty or contain digits. Please try again.");
                }
            } while (!validName);

            // Prompt for and validate last name
            do
            {
                Console.WriteLine("Enter your LastName:");
                lastName = Console.ReadLine();
                validName = Validations.IsValidName(lastName);
                if (!validName)
                {
                    Console.WriteLine("Name cannot be empty or contain digits. Please try again.");
                }
            } while (!validName);

            // Prompt for and validate email
            do
            {
                Console.WriteLine("Enter your email:");
                email = Console.ReadLine();
                validEmail = Validations.IsValidEmail(email);
                if (!validEmail)
                {
                    Console.WriteLine("Incorrect email address. Please try again.");
                }
            } while (!validEmail);

            // Prompt for and validate password
            do
            {
                Console.WriteLine("Enter your password:");
                password = Console.ReadLine();
                validPassword = Validations.IsValidPassword(password);
                if (!validPassword)
                {
                    Console.WriteLine("Password must contain at least 6 characters, one uppercase letter, one digit, and one special character. Please try again.");
                }
            } while (!validPassword);

            // Register the user
            UserMethods userMethods = new UserMethods();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            firstName = textInfo.ToTitleCase(firstName);
            lastName = textInfo.ToTitleCase(lastName);
            userMethods.Register(firstName, lastName, email, password);

            // Retrieve the newly registered user
            List<User> users = db.GetAllEntities<User>();
            Guid userId = users[^1].Id; // Takes the last recently registered userId

            // Prompt for account type
            Console.WriteLine("Enter the account type you want to open:");
            string accountType = Console.ReadLine();

            // Open the account for the newly registered user
            OpenAccount(userId, accountType);
        }

        public static void OpenAccount(Guid userId, string accountType)
        {
            BankApp_DbContext db = new BankApp_DbContext();
            List<User> users = db.GetAllEntities<User>();
            List<Account> accounts = db.GetAllEntities<Account>();

            string accountNumber = GenerateRandomAccountNumber();  // Generate the 10-digit account number
            float initialAccountBalance = 10;

            bool userFound = false;

            foreach (User user in users)
            {
                if (user.Id.Equals(userId))
                {
                    userFound = true;

                    // Check if the user already has an account
                    foreach (Account account in accounts)
                    {
                        if (account.userId.Equals(userId))
                        {
                            Console.WriteLine("You already have an account with us.");
                            return;
                        }
                    }

                    // If no account exists, create a new one
                    Account newAccount = new Account(userId, initialAccountBalance, accountNumber, accountType);
                    db.AddEntity(newAccount);
                    Console.WriteLine($"\nAccount created successfully for user {user.FirstName} with account number {accountNumber}.");
                    break;
                }
            }

            if (!userFound)
            {
                Console.WriteLine("Please register first before trying to create an account.");
            }
        }

    

        private static string GenerateRandomAccountNumber()
        {
            Random random = new Random();
            // Generate two 5-digit numbers and concatenate them
            int part1 = random.Next(10000, 100000); // Generates a 5-digit number
            int part2 = random.Next(10000, 100000); // Generates another 5-digit number
            return part1.ToString() + part2.ToString(); // Concatenate them to form a 10-digit number
        }


        public static void DisplayAccountInfo()
        {
            BankApp_DbContext db = new BankApp_DbContext();
            List<Account> accounts = db.GetAllEntities<Account>();
            List<User> users = db.GetAllEntities<User>();

            Console.WriteLine("Enter account identifier (first name for now):");
            string identifier = Console.ReadLine();

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            identifier = textInfo.ToTitleCase(identifier);

            int index = 0;
            int length = users.Count;
            List<AccountDisplay> accountDisplays = new List<AccountDisplay>();

            foreach (User user in users)
            {
                if (user.FirstName.Equals(identifier))
                {
                    foreach (Account account in accounts)
                    {
                        if (user.Id.Equals(account.userId))
                        {
                            accountDisplays.Add(new AccountDisplay
                            {
                                FullName = $"{user.FirstName} {user.LastName}",
                                AccountNumber = account.accountNumber,
                                AccountType = account.accountType,
                                AccountBalance = account.accountBalance,
                                UserId = account.userId
                            });
                        }
                    }
                }
                if (!user.FirstName.Equals(identifier) && index == length - 1)
                {
                    Console.WriteLine("User not found in database");
                }
                index++;
            }

            ConsoleTableBuilder
                .From(accountDisplays)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
        }


        public void showAllDb<T>(List<T> obj) where T : class
        {
            ConsoleTableBuilder
                .From(obj)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
        }

        private class AccountDisplay
        {
            public string FullName { get; set; }
            public string AccountNumber { get; set; }
            public string AccountType { get; set; }
            public float AccountBalance { get; set; }
            public Guid UserId { get; set; }
        }

    }
}
