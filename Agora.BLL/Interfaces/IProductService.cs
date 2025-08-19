using Agora.BLL.DTO;

namespace Agora.BLL.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAll();
        Task<IEnumerable<ProductDTO>> GetFilteredByName(string filter); // to be continued
        Task<IEnumerable<ProductDTO>> GetProductsBySeller(int sellerId);
        Task<IEnumerable<ProductDTO>> GetProductsByStore(int storeId);
        Task<IEnumerable<ProductDTO>> GetAllProductsByStore(int storeId);
        Task<IEnumerable<ProductDTO>> GetSimilarProducts(int productId);
        Task<List<ProductDTO>> GetFiltredProducts(int storeId, string field, string value);
        Task<ProductDTO> Get(int id);
        Task<ProductDTO> GetByName(string name);
        Task UpdateAllDiscountedPrices();
        Task Create(ProductDTO productDTO);
        Task Update(ProductDTO productDTO);
        Task Delete(int id);
    }
}
