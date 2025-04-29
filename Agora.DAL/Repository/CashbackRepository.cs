using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class CashbackRepository : IRepository<Cashback>
    {
        private AgoraContext db;
        public CashbackRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Cashback>> GetAll()
        {
            return db.Cashbacks;
        }

        public async Task<Cashback> Get(int id)
        {
            return await db.Cashbacks.FindAsync(id);
        }

        public async Task Create(Cashback cashback)
        {
            await db.Cashbacks.AddAsync(cashback);
        }

        public void Update(Cashback cashback)
        {
            db.Entry(cashback).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Cashback? cashback = await db.Cashbacks.FindAsync(id);
            if (cashback != null)
                db.Cashbacks.Remove(cashback);
        }
    }
}