using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class Promotion
    {
        public Promotion()
        {
            PromotionDetails = new HashSet<PromotionDetail>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public int? DiscountValue { get; set; }
        public string DiscountUnit { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Code { get; set; }
        public int? MaxDiscount { get; set; }
        public int? MinOrder { get; set; }
        public bool Activate { get; set; }

        public virtual ICollection<PromotionDetail> PromotionDetails { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
