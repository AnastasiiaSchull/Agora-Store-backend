using Agora.DAL.Entities;

namespace Agora.BLL.DTO
{
    public class SubcategoryDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO? CategoryDTO { get; set; }
    }
}
