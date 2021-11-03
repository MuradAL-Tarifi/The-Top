using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheTop.Models.ViewModel
{
    public class IndexViewModel
    {
        public Users Users { get; set; }
        public IEnumerable<Coupon> Coupons { get; set; }

        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Rate> Rates { get; set; }

        public IEnumerable<Category> Categorys { get; set; }
        public IEnumerable<Users> UsersList { get; set; }


        public IEnumerable<CartItem> CartItems { get; set; }

        public IEnumerable<FavoriteProduct> FavoriteProducts { get; set; }
        public IEnumerable<Notification> Notifications { get; set; }


    }
}
