using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class Geomancy
    {
        public Geomancy()
        {
            Products = new HashSet<Product>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
    }
}
