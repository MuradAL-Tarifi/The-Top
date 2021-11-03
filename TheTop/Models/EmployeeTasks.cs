using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class EmployeeTasks
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Users User { get; set; }
    }
}
