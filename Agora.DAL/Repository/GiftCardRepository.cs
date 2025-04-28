using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class GiftCardRepository : IRepository<GiftCard>
    {
        private AgoraContext db;
        public GiftCardRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<GiftCard>> GetAll()
        {
            return db.GiftCards;
        }

        public async Task<GiftCard> Get(int id)
        {
            return await db.GiftCards.FindAsync(id);
        }

        public async Task Create(GiftCard giftCard)
        {
            await db.GiftCards.AddAsync(giftCard);
        }

        public void Update(GiftCard giftCard)
        {
            db.Entry(giftCard).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            GiftCard? giftCard = await db.GiftCards.FindAsync(id);
            if (giftCard != null)
                db.GiftCards.Remove(giftCard);
        }
    }
}