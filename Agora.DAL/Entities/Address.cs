
namespace Agora.DAL.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string? Building { get; set; }
        public string? Appartement { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }

        public int? CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public virtual ICollection<Shipping>? Shipping { get; set; }
        public virtual ICollection<User>? User { get; set; }
    }
}
