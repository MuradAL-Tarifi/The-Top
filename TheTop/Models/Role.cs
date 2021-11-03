using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class Role
    {
        public Role()
        {
            EmployeeTasks = new HashSet<EmployeeTasks>();
            Users = new HashSet<Users>();
        }

        public int RoleId { get; set; }
        public string Role1 { get; set; }

        public virtual ICollection<EmployeeTasks> EmployeeTasks { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
