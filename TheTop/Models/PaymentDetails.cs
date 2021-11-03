using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class PaymentDetails
    {
        public PaymentDetails()
        {
            Payment = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public long? CardNumber { get; set; }
        public string Expiry { get; set; }
        public int? CardCode { get; set; }
        public decimal? Balance { get; set; }

        public virtual ICollection<Payment> Payment { get; set; }
    }
}
