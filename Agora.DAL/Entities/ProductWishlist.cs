using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.DAL.Entities
{
    public class ProductWishlist
    {
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int WishlistId { get; set; }
        public virtual Wishlist? Wishlist { get; set; }

        public DateTime DateAdded { get; set; } 
    }
}
