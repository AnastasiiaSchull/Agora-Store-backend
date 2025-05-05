using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.BLL.Interfaces
{
    public interface IStatisticsInitializer
    {
        Task InitializeEmptyStatsForStore(int storeId);
    }
}
