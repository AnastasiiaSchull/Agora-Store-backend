using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class SupportRepository: IRepository<Support>
    {
        private AgoraContext db;
        public SupportRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Support>> GetAll()
        {
            return db.Supports;
        }

        public async Task<Support> Get(int id)
        {
            return await db.Supports.FindAsync(id);
        }

        public async Task Create(Support support)
        {
            await db.Supports.AddAsync(support);
        }

        public void Update(Support support)
        {
            db.Entry(support).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Support? support = await db.Supports.FindAsync(id);
            if (support != null)
                db.Supports.Remove(support);
        }
    }
}
