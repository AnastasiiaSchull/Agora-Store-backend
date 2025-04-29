using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class DailyRevenueDTO
    {
        public DateOnly Date { get; set; }
        public string? DayOfWeek { get; set; }
        public decimal Revenue { get; set; }
    }
}
