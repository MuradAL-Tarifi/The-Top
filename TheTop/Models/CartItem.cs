using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class CartItem
    {
        public int Id { get; set; }
        public string Cartphoto { get; set; }
        public string CartName { get; set; }
        public int CartPrice { get; set; }
        public int? UserId { get; set; }
        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Users User { get; set; }
    }
}
