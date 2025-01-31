﻿using BankApp.Classes;
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
    public class TransactionHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TransactionType { get; set; } // e.g., "Deposit", "Withdrawal", "Transfer"
        public float Amount { get; set; }
        public DateTime Timestamp { get; set; }

        public TransactionHistory(Guid userId, string transactionType, float amount)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            TransactionType = transactionType;
            Amount = amount;
            Timestamp = DateTime.Now;
        }


    }   

}
