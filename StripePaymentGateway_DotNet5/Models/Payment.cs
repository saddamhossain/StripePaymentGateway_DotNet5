﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StripePaymentGateway_DotNet5.Models
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? BookId { get; set; }
        public int? Quantity { get; set; }
        public double? Amount { get; set; }
        public double? TotalAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Currency { get; set; }
        public string Intent { get; set; }
        public string StripeReference { get; set; }
        public string PayerId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ShippingAddress { get; set; }
        public string Description { get; set; }
        public string Tax { get; set; }
    }
}
