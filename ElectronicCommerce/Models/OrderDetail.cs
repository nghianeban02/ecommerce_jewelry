using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class OrderDetail
    {
        public string OrderDetailId { get; set; }
        public string OrderId { get; set; }
        public string ProductDetailId { get; set; }
        public int? Quantity { get; set; }
        public int? SalePrice { get; set; }

        public virtual OrderProduct Order { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
    }
}
