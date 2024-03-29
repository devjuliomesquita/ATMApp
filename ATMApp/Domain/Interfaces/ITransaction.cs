﻿using ATMApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.Domain.Interfaces
{
    public interface ITransaction
    {
        void InsertTransaction(
            long _UserBankAccountId,
            TransactionType _transactionType,
            decimal _trasactionAmount,
            string _desc);
        void ViewTransaction();
    }
}
