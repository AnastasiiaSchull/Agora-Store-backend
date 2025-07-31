using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class ReturnItemDetailsDTO
    {
        public int ReturnItemId { get; set; }
        public DateOnly? ReturnDate { get; set; }
        public string? Reason { get; set; }
        public string? Status { get; set; }

        public string? ProductImage { get; set; }
        public string? ProductName { get; set; }

        public int? OrderItemId { get; set; }
        public DateOnly? OrderDate { get; set; }
        public decimal? Price { get; set; }

        public string? UserName { get; set; }
        public string? UserSurname { get; set; }

        public string? Building { get; set; }
        public string? Appartement { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public string? Telephone { get; set; }
        public string? Email { get; set; }
    }
}
