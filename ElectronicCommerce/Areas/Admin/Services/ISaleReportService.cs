using ElectronicCommerce.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public interface ISaleReportService
    {
        public List<SaleReport> sp_ThongKeDoanhSo(string time);
        public List<SaleReportOption> sp_ThongKeDoanhSo_TuyChon(DateTime batdau, DateTime ketthuc, string httt);

    }
}
