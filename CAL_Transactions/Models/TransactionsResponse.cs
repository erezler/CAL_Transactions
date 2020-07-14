using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAL_Transactions.Models
{
    public class TransactionsResponse
    {
        public IEnumerable<MerchantTransaction> transactions;
    }
}