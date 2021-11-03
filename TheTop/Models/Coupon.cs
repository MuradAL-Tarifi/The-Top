using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class Coupon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Discount { get; set; }
        public string Picture { get; set; }
    }
}
