using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<List<Brand>> GetBrandsBySubcategoryOrCategoryAsync(int? subcategoryId = null, int? categoryId = null);
    }
}
