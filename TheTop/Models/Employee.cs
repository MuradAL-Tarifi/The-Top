using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Attendance = new HashSet<Attendance>();
            Challenge = new HashSet<Challenge>();
        }

        public int EmpId { get; set; }
        [Display(Name = "Discounts Description")]
        public string DiscountsDescription { get; set; }
        public int? Discounts { get; set; }
        public decimal? Salary { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int? Increase { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Attendance> Attendance { get; set; }
        public virtual ICollection<Challenge> Challenge { get; set; }
    }
}
