using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? UserId { get; set; }
        public string PaymentMonth { get; set; }
        public string PaymentYear { get; set; }
        public int? PaymentDetailsId { get; set; }

        public virtual PaymentDetails PaymentDetails { get; set; }
        public virtual Users User { get; set; }
    }
}
