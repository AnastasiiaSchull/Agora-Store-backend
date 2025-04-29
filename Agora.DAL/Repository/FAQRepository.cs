using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class FAQRepository : IRepository<FAQ>
    {
        private AgoraContext db;
        public FAQRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<FAQ>> GetAll()
        {
            return db.FAQs;
        }

        public async Task<FAQ> Get(int id)
        {
            return await db.FAQs.FindAsync(id);
        }

        public async Task Create(FAQ faq)
        {
            await db.FAQs.AddAsync(faq);
        }

        public void Update(FAQ faq)
        {
            db.Entry(faq).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            FAQ? faq = await db.FAQs.FindAsync(id);
            if (faq != null)
                db.FAQs.Remove(faq);
        }
    }
}