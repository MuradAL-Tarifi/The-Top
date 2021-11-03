﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TheTop.Models
{
    public partial class Attendance
    {
        public int Id { get; set; }
        public string LastLogin { get; set; }
        public string LastLogout { get; set; }
        public DateTime? Day { get; set; }
        public int? EmpId { get; set; }

        public virtual Employee Emp { get; set; }
    }
}
