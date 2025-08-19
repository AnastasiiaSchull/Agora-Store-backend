namespace Agora.BLL.DTO
{
    public class UpdateSellerPhoneNumberDTO
    {
        public int UserId { get; set; }
        public string NewPhoneNumber { get; set; } = string.Empty;
    }
}
