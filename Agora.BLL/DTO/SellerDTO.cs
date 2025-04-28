using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class SellerDTO
    {
        public int Id { get; set; }
        public float Rating { get; set; } = 0;

        public int? UserId { get; set; }
        //public UserDTO User { get; set; }
        //public ICollection<SellerReviewDTO> sellerReviews { get; set; }
    }
}
