using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class WishlistDTO
    {
        public int Id { get; set; }
        public DateOnly? DateAdded { get; set; }
        public CustomerDTO? Customer { get; set; }
        public List<int>? ProductIds { get; set; }
        public List<ProductDTO> Products { get; set; } = new();

    }
}
