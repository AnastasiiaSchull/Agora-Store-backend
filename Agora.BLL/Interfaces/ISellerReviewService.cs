using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface ISellerReviewService
    {
        Task<IEnumerable<SellerReviewDTO>> GetAll();
        Task<SellerReviewDTO> Get(int id);
        Task Create(SellerReviewDTO sellerReviewDTO);
        Task Update(SellerReviewDTO sellerReviewDTO);
        Task Delete(int id);
    }
}
