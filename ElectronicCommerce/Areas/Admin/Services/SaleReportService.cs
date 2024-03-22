using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public class SaleReportService : ISaleReportService
    {
        private DatabaseContext _db;
        public SaleReportService(DatabaseContext db)
        {
            _db = db;
        }

        public List<SaleReport> sp_ThongKeDoanhSo(string time)
        {
            return _db.SaleReports.FromSqlRaw("exec sp_ThongKeDoanhSo " + time).ToList();
        }
        public List<SaleReportOption> sp_ThongKeDoanhSo_TuyChon(DateTime batdau, DateTime ketthuc, string httt)
        {
            string formattedDateBD = batdau.ToString("yyyy/MM/dd");
            string formattedDateKT = ketthuc.ToString("yyyy/MM/dd");
            return _db.SaleReportOptions.FromSqlRaw($"exec sp_ThongKeDoanhSo_TuyChon '{batdau}' , '{ketthuc}', N'{httt}'").ToList();
        }
    }
}
