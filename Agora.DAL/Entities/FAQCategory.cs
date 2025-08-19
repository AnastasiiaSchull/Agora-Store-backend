
namespace Agora.DAL.Entities
{
    public class FAQCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<FAQ>? FAQs { get; set; }
    }
}
