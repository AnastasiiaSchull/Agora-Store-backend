using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class TopProductDTO
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int QuantitySold { get; set; }
    }
}
