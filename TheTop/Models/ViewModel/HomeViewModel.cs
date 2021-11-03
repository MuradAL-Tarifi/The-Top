using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheTop.Models.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categorys { get; set; }
        public IEnumerable<Users> Users { get; set; }
        public IEnumerable<Testimonial> Testimonials { get; set; }

    }
}
