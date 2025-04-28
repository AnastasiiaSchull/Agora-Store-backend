namespace Agora.BLL.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public string? Building { get; set; }
        public string? Appartement { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public int? CountryId { get; set; }
    }
}
