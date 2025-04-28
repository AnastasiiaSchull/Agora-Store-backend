
using System.Text.Json.Serialization;

namespace Agora.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? GoogleId { get; set; }

        public virtual ICollection<Address>? Addresses { get; set; }

        [JsonIgnore]
        public virtual Admin? Admin { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Seller? Seller { get; set; }
    }
}
