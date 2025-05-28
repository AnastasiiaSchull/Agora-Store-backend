using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<List<Brand>> GetBrandsBySubcategoryAsync(int subcategoryId);     
    }
}
