using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItem = new HashSet<CartItem>();
            FavoriteProduct = new HashSet<FavoriteProduct>();
            Notification = new HashSet<Notification>();
            Rate = new HashSet<Rate>();
        }

        public int ProductId { get; set; }
        [Display(Name = "Product Description")]
        public string Description { get; set; }
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Product Price")]
        public int Price { get; set; }
        public string Location { get; set; }
        public string Photo { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }
        public DateTime Expire { get; set; }

        public virtual Category Category { get; set; }
        public virtual Users User { get; set; }
        public virtual ICollection<CartItem> CartItem { get; set; }
        public virtual ICollection<FavoriteProduct> FavoriteProduct { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<Rate> Rate { get; set; }
    }
}
