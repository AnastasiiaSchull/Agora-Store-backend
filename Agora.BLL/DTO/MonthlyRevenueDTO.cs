using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Agora.BLL.DTO
{
    public class MonthlyRevenueDTO
    {
        public  string Date { get; set; }
        public decimal ThisMonth { get; set; }
        public decimal LastMonth { get; set; }
        
    }
}
