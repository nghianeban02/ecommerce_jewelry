using ElectronicCommerce.Models;
using System.Collections.Generic;

namespace ElectronicCommerce.Areas.Admin.ViewModels
{
    public class CommonPromotionModel
    {
        public Promotion promotion { get; set; }
        public List<PromotionDetail> promotionDetails { get; set; }
    }
}
