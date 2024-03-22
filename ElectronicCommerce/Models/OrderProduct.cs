using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class OrderProduct
    {
        public OrderProduct()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public string CustomerId { get; set; }
        public string AddressDelivery { get; set; }
        public int? Pay { get; set; }
        public DateTime? DatePay { get; set; }
        public string PayType { get; set; }
        public decimal? TotalPay { get; set; }
        public string OrderState { get; set; }
        public string PhoneNonAccount { get; set; }
        public string NameCusNonAccount { get; set; }
        public DateTime? ShipDate { get; set; }
        public int? ShipFee { get; set; }
        public string IdUser { get; set; }
        public string MailNonCus { get; set; }
        public string PromotionId { get; set; }
        public string CustomerTypeId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Promotion PromotionIdNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
