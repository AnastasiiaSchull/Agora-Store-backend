
namespace Agora.DAL.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Subcategory>? Subcategories { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
