using System;
using System.Collections.Generic;

#nullable disable

namespace ElectronicCommerce.Models
{
    public partial class PromotionDetail
    {
        public string IdPromotionDetail { get; set; }
        public string PromotionId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerTypeId { get; set; }
        

        public virtual CustomerType CustomerType { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Promotion Promotion { get; set; }
        
    }
}
