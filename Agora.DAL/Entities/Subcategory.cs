using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agora.DAL.Entities
{
    public class Subcategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
