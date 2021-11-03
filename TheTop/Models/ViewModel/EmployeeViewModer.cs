using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheTop.Models.ViewModel
{
    public class EmployeeViewModer
    {
        public Users Users { get; set; }
        public Employee Employee { get; set; }

        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Attendance> Attendances { get; set; }
        public IEnumerable<Challenge> Challenge { get; set; }
        public IEnumerable<EmployeeTasks> EmployeeTasks { get; set; }

    }
}
