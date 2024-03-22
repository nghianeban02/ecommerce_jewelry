using ElectronicCommerce.Areas.Admin.ViewModels;
using ElectronicCommerce.Models;
using ElectronicCommerce.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicCommerce.Areas.Admin.Services
{
    public class StoreService : IStoreService
    {
        private DatabaseContext _db;


        public StoreService(DatabaseContext db)
        {
            _db = db;
        }

        public List<Store> getAll()
        {
            return _db.Stores.FromSqlRaw("select * from stores").ToList();
           
        }
    }
}
