namespace Agora.BLL.DTO
{
    public class UpdateSellerEmailDTO
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; } = string.Empty;
    }

}
