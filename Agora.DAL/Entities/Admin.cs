
namespace Agora.DAL.Entities
{
    public class Admin
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
