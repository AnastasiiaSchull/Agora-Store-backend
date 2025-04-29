using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Agora.DAL.Repository
{
    public class CategoryRepository : IRepository<Category>
    {
        private AgoraContext db;
        public CategoryRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Category>> GetAll()
        {
            return db.Categories;
        }

        public async Task<Category> Get(int id)
        {
            return await db.Categories.FindAsync(id);
        }

        public async Task Create(Category category)
        {
            await db.Categories.AddAsync(category);
        }

        public void Update(Category category)
        {
            db.Entry(category).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Category? category = await db.Categories.FindAsync(id);
            if (category != null)
                db.Categories.Remove(category);
        }

        public async Task<IEnumerable<Category>> Find(Expression<Func<Category, bool>> predicate)
        {
            return await db.Categories.Where(predicate).ToListAsync();
        }
    }
}
