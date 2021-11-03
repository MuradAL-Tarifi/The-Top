using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheTop.Models.ViewModel
{
    public class CheckoutViewModel
    {
        public Users Users { get; set; }
        public Payment Payment { get; set; }
        public IEnumerable<CartItem>  CartItems { get; set; }
    }
}
