using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class ProductPrice
    {
        public ProductPrice()
        {
            ProductDetails = new HashSet<ProductDetail>();
        }

        public string ProductPriceId { get; set; }
        public int? BasePrice { get; set; }
        public int SalePrice { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? InActive { get; set; }

        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
    }
}
