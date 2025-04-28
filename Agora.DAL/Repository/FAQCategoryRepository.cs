using Agora.DAL.EF;
using Agora.DAL.Entities;
using Agora.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Agora.DAL.Repository
{
    public class FAQCategoryRepository : IRepository<FAQCategory>
    {
        private AgoraContext db;
        public FAQCategoryRepository(AgoraContext context)
        {
            this.db = context;
        }

        public async Task<IQueryable<FAQCategory>> GetAll()
        {
            return db.FAQCategories;
        }

        public async Task<FAQCategory> Get(int id)
        {
            return await db.FAQCategories.FindAsync(id);
        }

        public async Task Create(FAQCategory faqCategory)
        {
            await db.FAQCategories.AddAsync(faqCategory);
        }

        public void Update(FAQCategory faqCategory)
        {
            db.Entry(faqCategory).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            FAQCategory? faqCategory = await db.FAQCategories.FindAsync(id);
            if (faqCategory != null)
                db.FAQCategories.Remove(faqCategory);
        }
    }
}