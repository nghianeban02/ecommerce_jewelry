using ElectronicCommerce.Areas.Admin.Services;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/salereport")]
    public class AdminSaleReportController : Controller
    {
        IBaseRepository<OrderProduct> _baseRepoOrderProduct;
        ISaleReportService _saleReportService;
        public AdminSaleReportController(ISaleReportService saleReportService, IBaseRepository<OrderProduct> baseRepoOrderProduct)
        {
            _saleReportService = saleReportService;
            _baseRepoOrderProduct = baseRepoOrderProduct;
        }

        [Route("index")]
        [Route("")]
        //[Route("~/")]
        public IActionResult Index()
        {
            // lấy danh sách các trạng thái đơn hàng và httt đến view
            List<string> lstStatus = new List<string>();
            List<string> lstHttt = new List<string>();
            HashSet<string> visitedIds = new HashSet<string>();
            foreach (var item in _baseRepoOrderProduct.GetAll().ToList())
            {
                if (!visitedIds.Contains(item.PayType))
                {
                    lstHttt.Add(item.PayType);
                    visitedIds.Add(item.PayType);
                }
            }

            ViewBag.lstHttt = lstHttt;
            return View();
        }

        [HttpGet]
        [Route("GetReport")]
        public IActionResult GetReportByYear(string time)
        {
           
            var lstData = _saleReportService.sp_ThongKeDoanhSo(time);
            return new JsonResult(lstData);
        }

        [HttpGet]
        [Route("getReportByOption")]
        public IActionResult getReportByOption(DateTime batdau, DateTime ketthuc, string httt)
        {
            var lstData = _saleReportService.sp_ThongKeDoanhSo_TuyChon(batdau,ketthuc, httt);
            
            return new JsonResult(lstData);
        }


    }
}
