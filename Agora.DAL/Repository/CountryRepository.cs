using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class CountryRepository : IRepository<Country>
    {
        private AgoraContext db;
        public CountryRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<Country>> GetAll()
        {
            return db.Countries;
        }

        public async Task<Country> Get(int id)
        {
            return await db.Countries.FindAsync(id);
        }

        public async Task Create(Country country)
        {
            await db.Countries.AddAsync(country);
        }

        public void Update(Country country)
        {
            db.Entry(country).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            Country? country = await db.Countries.FindAsync(id);
            if (country != null)
                db.Countries.Remove(country);
        }
    }
}