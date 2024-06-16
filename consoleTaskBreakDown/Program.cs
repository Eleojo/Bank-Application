using BankApp.Classes;
using BankApp.Database;
using BankApp.Methods;
using System.Globalization;
using System.Runtime;

namespace BankApp
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        private void Run()
        {
            // Instantiations
            BankApp_DbContext db = new BankApp_DbContext();
            AccountMethods show = new AccountMethods();
            Transactions transactions = new Transactions();


            Console.WriteLine("WELCOME TO MY BANK APP\n\n");

            while (true)
            {
                ShowMenu();

                string userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Please input a valid option.");
                }
                else if (userInput == "1")
                {
                   AccountMethods.RegisterAndOpenAccount();
                }
                else if (userInput == "2")
                {
                    transactions.WithdrawMoney();
                }
                else if (userInput == "3")
                {
                    transactions.DepositMoney();
                }
                else if (userInput == "4")
                {
                    AccountMethods.DisplayAccountInfo();
                }
                else if(userInput == "5")
                {
                    Console.WriteLine("Enter amount to transfer");
                    string readAmount = Console.ReadLine();
                    float amount = float.Parse(readAmount);
                    transactions.Transfer(amount);
                }
                else if(userInput == "6")
                {
                    List<User> users = db.GetAllEntities<User>();
                    List<Account> accounts = db.GetAllEntities<Account>();
                    
                    Console.WriteLine("What Database would you like to see");
                    string dbType = Console.ReadLine().ToLower();
                    if(dbType == "users")
                    {
                        show.showAllDb(users);

                    }
                    else
                    {
                        show.showAllDb(accounts);

                    }

                }
                else if(userInput == "7")
                {
                    TransactionHistoryMethods transactionHistoryMethods = new TransactionHistoryMethods();
                    transactionHistoryMethods.ViewTransactionHistory();
                }
                else if (userInput == "8")
                {
                    Console.WriteLine("Thank you for banking with us.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option, please try again.");
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("\nWhat would you like to do today\n");
            Console.WriteLine("Press 1 to Register and Open an Account.");
            Console.WriteLine("Press 2 to Withdraw money.");
            Console.WriteLine("Press 3 to Deposit money.");
            Console.WriteLine("Press 4 to Display Account info.");
            Console.WriteLine("Press 5 to transfer.");
            Console.WriteLine("Press 6 to Show All Accounts.");
            Console.WriteLine("Press 7 to Show User Transaction History.");
            Console.WriteLine("Press 8 to Exit.");
        }

    }
}
