using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class PaymentMethodRepository: IRepository<PaymentMethod>
    {
        private AgoraContext db;
        public PaymentMethodRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<PaymentMethod>> GetAll()
        {
            return db.PaymentMethods;
        }

        public async Task<PaymentMethod> Get(int id)
        {
            return await db.PaymentMethods.FindAsync(id);
        }

        public async Task Create(PaymentMethod paymentMethod)
        {
            await db.PaymentMethods.AddAsync(paymentMethod);
        }

        public void Update(PaymentMethod paymentMethod)
        {
            db.Entry(paymentMethod).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            PaymentMethod? paymentMethod = await db.PaymentMethods.FindAsync(id);
            if (paymentMethod != null)
                db.PaymentMethods.Remove(paymentMethod);
        }
    }
}
