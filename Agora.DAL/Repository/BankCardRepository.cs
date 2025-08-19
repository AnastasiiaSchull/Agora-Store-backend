using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class BankCardRepository : IRepository<BankCard>
    {
        private AgoraContext db;
        public BankCardRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<BankCard>> GetAll()
        {
            return db.BankCards;
        }

        public async Task<BankCard> Get(int id)
        {
            return await db.BankCards.FindAsync(id);
        }

        public async Task Create(BankCard bankCard)
        {
            await db.BankCards.AddAsync(bankCard);
        }

        public void Update(BankCard bankCard)
        {
            db.Entry(bankCard).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            BankCard? bankCard = await db.BankCards.FindAsync(id);
            if (bankCard != null)
                db.BankCards.Remove(bankCard);
        }
    }
}