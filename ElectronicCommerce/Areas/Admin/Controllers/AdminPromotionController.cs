using AspNetCoreHero.ToastNotification.Abstractions;
using ElectronicCommerce.Areas.Admin.Helpers;
using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/promotion")]
    public class AdminPromotionController : Controller
    {
        private IBaseRepository<Promotion> _baseRepoPromotion;
        private IBaseRepository<PromotionDetail> _baseRepoPromotionDetail;
        private IBaseRepository<CustomerType> _baseRepoCustomerType;

        private INotyfService _notyfService;

        public AdminPromotionController(IBaseRepository<Promotion> baseRepository, INotyfService notyfService,
            IBaseRepository<PromotionDetail> baseRepoPromotionDetail, IBaseRepository<CustomerType> baseRepoCustomerType)
        {
            _baseRepoPromotion = baseRepository;
            _notyfService = notyfService;
            _baseRepoPromotionDetail = baseRepoPromotionDetail;
            _baseRepoCustomerType = baseRepoCustomerType;
        }

        [Route("index")]
        [Route("")]
        //[Route("~/")]
        public IActionResult Index()
        {
            List<Promotion> promotions = _baseRepoPromotion.GetAll().ToList();
            ViewBag.promotions = promotions;
            return View("index", new Promotion());
        }

        
        [Route("add")]
        [HttpPost]
        public IActionResult Add(Promotion promotion)
        {
            promotion.Id = "PMT" + OrderCode.RandomString(3);
            promotion.Activate = false;
            _baseRepoPromotion.Insert(promotion);
            _baseRepoPromotion.Save();
            return RedirectToAction("index");
        }

        [Route("detail/{id}")]
        // trang chi tiet khuyen mai
        public IActionResult Detail(string id, string message)
        {
            if (message != null)
            {
                ViewBag.msg = message;
            }
            var commonPromotion = new CommonPromotionModel();
            var promotion = _baseRepoPromotion.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(id));
            ViewBag.promotion = promotion;
            commonPromotion.promotion = promotion;
            //hien thi doi tuong khach hang
            List<PromotionDetail> promotionDetails = _baseRepoPromotionDetail.GetAll().ToList().Where(p => p.PromotionId.Equals(id)).ToList();
            ViewBag.promotionDetails = promotionDetails;
            // danh sách loại khách hàng
            List<CustomerType> lstAddCustomer = new List<CustomerType>();
            foreach(var item in _baseRepoCustomerType.GetAll().ToList())
			{
                var temp = 0;
                foreach(var pd in promotionDetails)
				{
                    if (item.Id == pd.CustomerTypeId) temp += 1; 
				}
                if(temp == 0)
				{
                    lstAddCustomer.Add(item);
				}
			}
            ViewBag.CustomerType = lstAddCustomer;
            
            return View("detail", commonPromotion);
        }
        // cap nhat trang thai
        [Route("updateactivate")]
        [HttpPost]
        public IActionResult UpdateActivate(string promotion_id)
        {
            var promotion = _baseRepoPromotion.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(promotion_id));
            if (promotion.Activate)
            {
                promotion.Activate = false;
            }
            else
            {
               promotion.Activate = true;
            }

            _baseRepoPromotion.Update(promotion);
            _baseRepoPromotion.Save();
            return new JsonResult(new { message = "Success" });
        }


        //cap nhat khuyen mai
        [HttpPost]
        [Route("update")]
        public IActionResult Update(CommonPromotionModel commonPromotionModel)
        {
            var promotion = _baseRepoPromotion.GetAll().ToList().SingleOrDefault(p => p.Id.Equals(commonPromotionModel.promotion.Id));
            promotion.Name = commonPromotionModel.promotion.Name;
            promotion.Description = commonPromotionModel.promotion.Description;
            promotion.DiscountValue = commonPromotionModel.promotion.DiscountValue;
            promotion.DiscountUnit = commonPromotionModel.promotion.DiscountUnit;
            promotion.StartDate = commonPromotionModel.promotion.StartDate;
            promotion.EndDate = commonPromotionModel.promotion.EndDate;
            promotion.Code = commonPromotionModel.promotion.Code;
            promotion.MaxDiscount = commonPromotionModel.promotion.MaxDiscount;
            promotion.MinOrder = commonPromotionModel.promotion.MinOrder;
            _baseRepoPromotion.Update(promotion);
            _baseRepoPromotion.Save();
            return RedirectToAction("detail", new { id = promotion.Id });
        }

        // them đối tượng khách hàng áp dụng khuyến mãi
        [HttpPost]
        [Route("addCustomerType")]
        public IActionResult addCustomerType(string customer_type_id, string promotion_id)
        {
            if(customer_type_id == null)
			{
                return RedirectToAction("detail", new { id = promotion_id });
            }
            PromotionDetail promotionDetail = new PromotionDetail();
            promotionDetail.IdPromotionDetail = "PRD" + OrderCode.RandomString(3);
            promotionDetail.PromotionId = promotion_id;
            promotionDetail.CustomerId = null;
            promotionDetail.CustomerTypeId = customer_type_id;
            _baseRepoPromotionDetail.Insert(promotionDetail);
            _baseRepoPromotionDetail.Save();
          
            return RedirectToAction("detail", new { id = promotion_id });

        }

        // xóa đối tượng khuyến mãi
     
        [Route("deleteCustomerType/{id}")]
        public IActionResult deleteCustomerType(string id)
        {
            List<PromotionDetail> promotionDetail = _baseRepoPromotionDetail.GetAll().ToList().Where(p => p.IdPromotionDetail.Equals(id)).ToList();
            string promotionId = promotionDetail[0].PromotionId;
            _baseRepoPromotionDetail.Delete(id);
            _baseRepoPromotionDetail.Save();
            return RedirectToAction("detail", new { id = promotionId , message = "sua thanh cong" });
        }

    }
}
