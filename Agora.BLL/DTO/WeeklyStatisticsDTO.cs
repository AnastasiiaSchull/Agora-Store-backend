using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.DTO
{
    public class WeeklyStatisticsDTO
    {
        public DateOnly Date { get; set; }
        public string DayOfWeek { get; set; }
        public int QuantityOfSales { get; set; }
    }
}
