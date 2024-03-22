using System;
using System.Collections.Generic;
using ElectronicCommerce.Models;

namespace ElectronicCommerce.Areas.Customer.Models
{
    public class ProductDetailDisplay
    {
        public string NAME { get; set; }
        public string CODE { get; set; }
        public int PRICE { get; set; }
        public string MAIN_STONE { get; set; }
        public string SUB_STONE { get; set; }
        public string COLOR { get; set; }
        public string THUMB_NAIL { get; set; }
        public List<Image> IMAGES { get; set; }
        public string GEOMANCY { get; set; }
        public int DISCOUNT_VALUE { get; set; }
        public string UNIT { get; set; }
        public double PRICE_AFTER_DISCOUNT { get; set; }
        public int countProduct {  get; set; }  

    }
}
