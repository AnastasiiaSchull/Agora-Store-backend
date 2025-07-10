using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<IEnumerable<Discount>> GetActiveWithRelations();
    }
}
