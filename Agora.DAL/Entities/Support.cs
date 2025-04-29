using Agora.Enums;

namespace Agora.DAL.Entities
{
    public class Support
    {
        public int Id { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public SupportStatus Status { get; set; }                                           
        public DateOnly Date { get; set; }

        public virtual Customer? Customer { get; set; }
    }
}
