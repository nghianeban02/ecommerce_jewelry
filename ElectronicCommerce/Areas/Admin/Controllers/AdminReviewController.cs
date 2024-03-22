using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/review")]
    public class AdminReviewController : Controller
    {
        // GET: /<controller>/
        private IBaseRepository<Review> _baseRepoReview;

        public AdminReviewController(IBaseRepository<Review> baseRepoReview)
        {
            _baseRepoReview = baseRepoReview;
        }

        [HttpGet]
        [Route("index/{id}")]
        public IActionResult Index(string id)
        {
            var reviews = _baseRepoReview.GetAll().ToList().Where(i => i.ProductId.Equals(id)).ToList();

            ViewBag.reviews = reviews;

            return View("index");
        }

        [HttpPost]
        [Route("accept")]
        public IActionResult Accept(string review_id)
        {
            var review = _baseRepoReview.GetById(review_id);
            review.Is_Update = true;
            _baseRepoReview.Update(review);
            _baseRepoReview.Save();
            return RedirectToAction("index", new { id = review.ProductId });
        }
    }
}
