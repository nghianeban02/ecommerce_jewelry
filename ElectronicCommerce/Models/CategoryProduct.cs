using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class CategoryProduct
    { 
        public CategoryProduct()
        {
            InverseParent = new HashSet<CategoryProduct>();
            Products = new HashSet<Product>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }

        public virtual CategoryProduct Parent { get; set; }
        public virtual ICollection<CategoryProduct> InverseParent { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
