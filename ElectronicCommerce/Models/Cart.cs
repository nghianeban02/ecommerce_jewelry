using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class Cart
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public int? Quantity { get; set; }
        public string OrderId { get; set; }
        public string ProductDetailId { get; set; }
        public Boolean Is_Check { get; set; }
        public int? SavePrice { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual OrderProduct Order { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
    }
}
