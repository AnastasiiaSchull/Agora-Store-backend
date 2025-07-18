using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.DAL.Entities;

namespace Agora.DAL.Interfaces
{
    public interface IGiftCardRepository
    {
        Task<IQueryable<GiftCard>> GetAll();
        Task<GiftCard> Get(int id);
        Task<GiftCard> GetByCode(string code);
        Task Create(GiftCard item);
        void Update(GiftCard item);
        Task Delete(int id);
    }
}
