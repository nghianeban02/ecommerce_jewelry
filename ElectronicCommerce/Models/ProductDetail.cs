using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string ProductDetailId { get; set; }
        public string ProductId { get; set; }
        public int? Quantity { get; set; }
        public int? ImportQuantity { get; set; }
        public string ProductPriceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int Size { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductPrice ProductPrice { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
