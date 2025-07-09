using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface ISellerService
    {
        Task<IQueryable<SellerDTO>> GetAll();
        Task<SellerDTO> Get(int id);
        Task<List<int>> GetAllSellerIds();
        Task<int> Create(SellerDTO sellerDTO);
        Task Update(SellerDTO sellerDTO);
        Task Delete(int id);
        Task BlockSeller(int sellerId);
        Task UnblockSeller(int sellerId);
    }
}
