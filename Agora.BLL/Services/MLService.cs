using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agora.BLL.Interfaces;
using Agora_BLL;
using Microsoft.ML;
using static Agora_BLL.MLSearchByImage;

namespace Agora.BLL.Services
{
    public class MLService : IMLService
    {
        public IOrderedEnumerable<KeyValuePair<string, float>> PredictAllLabels(byte[] imagePath)
        {
            var input = new ModelInput
            {
                ImageSource = imagePath 
            };
            IOrderedEnumerable<KeyValuePair<string, float>> predictions = MLSearchByImage.PredictAllLabels(input);
            return predictions;
        }
    }
}
