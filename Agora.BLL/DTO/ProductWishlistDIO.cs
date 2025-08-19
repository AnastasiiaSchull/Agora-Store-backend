using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class ProductWishlistDTO
    {
        public int ProductId { get; set; }
        public int WishlistId { get; set; }
        public DateTime DateAdded { get; set; }
        //public ProductDTO? Product { get; set; }        
    }
}
