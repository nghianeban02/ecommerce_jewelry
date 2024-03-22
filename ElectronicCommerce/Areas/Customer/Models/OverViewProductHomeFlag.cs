using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicCommerce.Areas.Customer.Models
{
    public class OverViewProductHomeFlag
    {
        public string PRODUCT_ID { get; set; }
        public string IMAGE { get; set; }
        public string NAME { get; set; }
        public int PRICE { get; set; }
        public int? DISCOUNT_VALUE { get; set; }
        public string? UNIT { get; set; }
        public bool? ACTIVE { get; set; }
        public string? DIS_NAME { get; set; }

    }
}
