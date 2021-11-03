using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace TheTop.Models
{
    public partial class Users
    {
        public Users()
        {
            CartItem = new HashSet<CartItem>();
            Employee = new HashSet<Employee>();
            EmployeeTasks = new HashSet<EmployeeTasks>();
            FavoriteProduct = new HashSet<FavoriteProduct>();
            Notification = new HashSet<Notification>();
            Payment = new HashSet<Payment>();
            Product = new HashSet<Product>();
            Rate = new HashSet<Rate>();
        }

        public int UserId { get; set; }
        [Display(Name = "User Name")]
        public string UsarName { get; set; }
        [StringLength(100, ErrorMessage = "Minimum 6 and maximum 100 charaters are allwed", MinimumLength = 6)]
        public string Password { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string UserPhoto { get; set; }
        public int? RoleId { get; set; }
        public bool? RememberMe { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Status { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<CartItem> CartItem { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<EmployeeTasks> EmployeeTasks { get; set; }
        public virtual ICollection<FavoriteProduct> FavoriteProduct { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<Rate> Rate { get; set; }
    }
}
