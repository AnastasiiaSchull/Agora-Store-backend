using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class UpdateSellerEmailDTO
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; } = string.Empty;
    }

}
