using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;

namespace CAL_Transactions.Models
{
    public class MerchantTransaction
    {
        public string id;
        public string merchantName;
        public DateTime purchaseDate;
        public double amount;
        public string symbol;
        public DateTime debitDate;
        public string merchantAddress;
        public int numOfPayments;
        public string transTypeComment;
        public string transactionType;
        public DateTime insertDate;
    }
}