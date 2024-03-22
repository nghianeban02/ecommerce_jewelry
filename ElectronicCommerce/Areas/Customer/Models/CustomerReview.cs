using System;
namespace ElectronicCommerce.Areas.Customer.Models
{
    public class CustomerReview
    {
        public string CUSTOMER_NAME { get; set; }
        public int RATE { get; set; }
        public string CUSTOMER_ID { get; set; }
        public DateTime CREATED { get; set; }
        public bool IS_UPDATE { get; set; }
        public string CONTENT { get; set; }
        public string TITLE { get; set; }

    }
}
