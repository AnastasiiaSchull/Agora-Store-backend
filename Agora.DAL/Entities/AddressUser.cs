namespace Agora.DAL.Entities
{
    public class AddressUser
    {
        public int AddressesId { get; set; }
        public int UserId { get; set; }
        public virtual Address Address { get; set; }
        public virtual User User { get; set; }
    }
}
