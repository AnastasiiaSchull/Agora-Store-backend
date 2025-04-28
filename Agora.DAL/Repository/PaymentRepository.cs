using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class PaymentRepository: IRepository<Payment>
    {
        private AgoraContext db;
        public PaymentRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Payment>> GetAll()
        {
            return db.Payments;
        }

        public async Task<Payment> Get(int id)
        {
            return await db.Payments.FindAsync(id);
        }

        public async Task Create(Payment payment)
        {
            await db.Payments.AddAsync(payment);
        }

        public void Update(Payment payment)
        {
            db.Entry(payment).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Payment? payment = await db.Payments.FindAsync(id);
            if (payment != null)
                db.Payments.Remove(payment);
        }
    }
}
