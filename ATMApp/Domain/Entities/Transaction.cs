using ATMApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.Domain.Entities
{
    public class Transaction
    {
        public long TransactionId { get; set; }
        public long UserBankAcconuntId { get; set; }
        public DateTime TrasactionDate { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Description { get; set; }
        public decimal TrasactionAmount { get; set; }
    }
}
