using Microsoft.AspNetCore.Mvc;

namespace ElectronicCommerce.Areas.Customer.Controllers
{
    public class StoreController : Controller
    {
        [Area("customer")]
        [Route("customer/store")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
