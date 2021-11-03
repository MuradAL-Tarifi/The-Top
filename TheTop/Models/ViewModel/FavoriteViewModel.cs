using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheTop.Models.ViewModel
{
    public class FavoriteViewModel
    {
        public Users Users { get; set; }

        public IEnumerable<FavoriteProduct> favoriteProducts { get; set; }
    }
}
