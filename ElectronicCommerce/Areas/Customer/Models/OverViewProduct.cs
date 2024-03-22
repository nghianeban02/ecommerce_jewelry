using System;
namespace ElectronicCommerce.Areas.Customer.Models
{
    public class OverViewProduct
    {
        public string THUMB_NAIL { get; set; }
        public string NAME { get; set; }
        public int PRICE { get; set; }
        public double PRICE_AFTER_DISCOUNT { get; set; }
        public string CODE { get; set; }
        public string CATE_NAME { get; set; }
        public int CountProduct { get; set; }
    }
}
