using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Agora.BLL.DTO
{
    public class CategorySalesDTO
    {
        public string CategoryName { get; set; }
        public int QuantityOfSales { get; set; }
    }
}
