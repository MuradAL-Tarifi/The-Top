using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheTop.Models.ViewModel
{
    public class AdminViewModel
    {
        public Users User { get; set; }

        public IEnumerable<Payment> Payments { get; set; }
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Employee> Employee { get; set; }
        public IEnumerable<Challenge> Challenge { get; set; }
        public IEnumerable<EmployeeTasks> EmployeeTasks { get; set; }
        public IEnumerable<Testimonial> Testimonials { get; set; }

        public EmployeeTasks EmployeeTask { get; set; }

        public IEnumerable<Users> Users { get; set; }
        public IEnumerable<Role> Roles { get; set; }


    }
}
