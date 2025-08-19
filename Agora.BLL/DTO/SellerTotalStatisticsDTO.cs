using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class SellerTotalStatisticsDTO
    {
        public int TotalProducts { get; set; }
        public int TotalOrderItems { get; set; }
        public string TotalRevenue { get; set; }
        public int TotalCustomers { get; set; }
        public float Rating { get; set; }

    }
}
