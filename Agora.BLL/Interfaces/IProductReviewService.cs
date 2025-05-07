using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IProductReviewService
    {
        Task<IEnumerable<ProductReviewDTO>> GetAll();
        Task<IEnumerable<ProductReviewDTO>> GetReviewsByStoreId(int storeId);
        Task<IEnumerable<ProductReviewDTO>> GetFilteredReviewsByStoreId(int storeId, string field, string value);
        Task<ProductReviewDTO> Get(int id);
        Task Create(ProductReviewDTO productReviewDTO);
        Task Update(ProductReviewDTO productReviewDTO);
        Task Delete(int id);
    }
}
