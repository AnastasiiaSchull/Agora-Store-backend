using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Agora_BLL.MLSearchByImage;

namespace Agora.BLL.Interfaces
{
    public interface IMLService
    {
        IOrderedEnumerable<KeyValuePair<string, float>> PredictAllLabels( byte[] imagePath);

    }
}
