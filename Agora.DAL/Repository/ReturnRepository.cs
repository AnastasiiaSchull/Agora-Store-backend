using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class ReturnRepository: IRepository<Return>
    {
        private AgoraContext db;
        public ReturnRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Return>> GetAll()
        {
            return db.Returns;
        }

        public async Task<Return> Get(int id)
        {
            return await db.Returns.FindAsync(id);
        }

        public async Task Create(Return _return)
        {
            await db.Returns.AddAsync(_return);
        }

        public void Update(Return _return)
        {
            db.Entry(_return).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Return? _return = await db.Returns.FindAsync(id);
            if (_return != null)
                db.Returns.Remove(_return);
        }
    }
}
