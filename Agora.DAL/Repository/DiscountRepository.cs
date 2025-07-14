using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private AgoraContext db;
        public DiscountRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Discount>> GetAll()
        {
            return db.Discounts;
        }

        public async Task<Discount> Get(int id)
        {
            return await db.Discounts.FindAsync(id);
        }

        public async Task<IEnumerable<Discount>> GetActiveWithRelations()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            return await db.Discounts
                     .AsNoTracking()
                .Include(d => d.Products)
                .Include(d => d.Brands)
                .Include(d => d.Categories)
                .Include(d => d.Subcategories)
                .Where(d => d.StartDate <= today && d.EndDate >= today)
                .ToListAsync();
        }

        public async Task Create(Discount discount)
        {
            await db.Discounts.AddAsync(discount);
        }

        public void Update(Discount discount)
        {
            db.Entry(discount).State = EntityState.Modified;
        }

        public async Task<Discount> GetWithRelations(int id)
        {
            return await db.Discounts
                .Include(d => d.Brands)
                .Include(d => d.Categories)
                .Include(d => d.Subcategories)
                .Include(d => d.Products)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        //если у скидки нет связей (с брендами, категориями, подкатегориями)
        public async Task Delete(int id)
        {
            Discount? discount = await db.Discounts.FindAsync(id);
            if (discount != null)
                db.Discounts.Remove(discount);
        }

        //если у скидки есть связи (с брендами, категориями, подкатегориями)
        public void Delete(Discount discount)
        {
            db.Discounts.Remove(discount);
        }

    }
}