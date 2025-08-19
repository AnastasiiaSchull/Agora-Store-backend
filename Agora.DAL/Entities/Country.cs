
namespace Agora.DAL.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Address>? Address { get; set; }
    }
}
