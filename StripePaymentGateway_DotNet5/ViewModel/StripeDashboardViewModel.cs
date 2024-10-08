﻿using Stripe;
using System.Collections.Generic;

namespace StripePaymentGateway_DotNet5.ViewModel
{
    public class StripeDashboardViewModel
    {
        public Balance Balance { set; get; }
        public List<Product> Products { set; get; }
        public List<BalanceTransaction> BalanceTransactions { set; get; }
        public List<Customer> Customers { set; get; }
        public List<Charge> Chages { set; get; }
        public List<Dispute> Disputs { set; get; }
        public List<Refund> Refunds { set; get; }     
    }
}
