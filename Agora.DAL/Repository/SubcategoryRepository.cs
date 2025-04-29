using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Agora.DAL.Repository
{
    public class SubcategoryRepository: IRepository<Subcategory>
    {
        private AgoraContext db;
        public SubcategoryRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Subcategory>> GetAll()
        {
            return db.Subcategories;
        }

        public async Task<Subcategory> Get(int id)
        {
            return await db.Subcategories.FindAsync(id);
        }

        public async Task Create(Subcategory subcategory)
        {
            await db.Subcategories.AddAsync(subcategory);
        }

        public void Update(Subcategory subcategory)
        {
            db.Entry(subcategory).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Subcategory? subcategory = await db.Subcategories.FindAsync(id);
            if (subcategory != null)
                db.Subcategories.Remove(subcategory);
        }
    }
}
