using Agora.DAL.Entities;

namespace Agora.BLL.DTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? CashbackId { get; set; }

        public UserDTO? UserDTO { get; set; }
    }
}
