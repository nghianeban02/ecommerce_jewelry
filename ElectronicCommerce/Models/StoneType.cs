using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class StoneType
    {
        public StoneType()
        {
            ProductMainStones = new HashSet<Product>();
            ProductSubStones = new HashSet<Product>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Product> ProductMainStones { get; set; }
        public virtual ICollection<Product> ProductSubStones { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
    }
}
