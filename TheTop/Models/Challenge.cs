using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class Challenge
    {
        public int TaskId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int? EmpId { get; set; }
        public bool IsActive { get; set; }

        public virtual Employee Emp { get; set; }
    }
}
