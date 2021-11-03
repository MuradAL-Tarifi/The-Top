using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheTop.Models.ViewModel
{
    public class AccountantViewModel
    {
        public IEnumerable<Users> Users { get; set; }
        public Users User { get; set; }

        public IEnumerable<Employee> Employees { get; set; }
    }
}
