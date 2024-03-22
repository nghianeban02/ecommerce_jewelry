using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public class ProductService:IProductService
    {
        private DatabaseContext _db;

        public ProductService(DatabaseContext db)
        {
            _db = db;
        }

        public List<OverViewProductHomeFlag> findAllHomeFlagProducts()
        {
            var result = _db.OverViewProductHomeFlags.FromSqlRaw("[dbo].[sp_findall_home_flag_product_with_min_price]").ToList();

            var pds = _db.ProductDetails.ToList();

            // Kiem tra san pham het hang neu so luong = 0 thi khong hien thi

            for (int i = 0; i < result.Count; i++)
            {
                if(totalQuantityOfProduct(result[i].PRODUCT_ID) ==0)
                {
                    result.Remove(result[i]);
                }
            }

            return result;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        // Kiem tra so luong ton cua san pham trong kho
        private int totalQuantityOfProduct(string product_id)
        {
            var pds = _db.ProductDetails.ToList();
            return (int)pds.Where(i => i.ProductId.Equals(product_id)).ToList().Sum(i => i.Quantity);
        }

        public List<int> findAllSizeOfProducts(string id)
        {
            var result =_db.ProductDetails.ToList().Where(i => i.ProductId == id).Select(i => i.Size).ToList();

            // Kiem tra neu so luong ton cua san pham cung size = 0 thi khong hien thi
            for (int i = 0; i < result.Count; i++)
            {
               if(!checkQuantityProductWithSize(id, result[i]))
                {
                    result.Remove(result[i]);
                }
            }

            return result;
        }

        private bool checkQuantityProductWithSize(string product_id, int size)
        {
            var pds = _db.ProductDetails.ToList().SingleOrDefault(p=> p.ProductId.Equals(product_id) && p.Size == size);
            if(pds.Quantity == 0)
            {
                return false;
            }
            return true;
        }

        public OverViewProductHomeFlag findProductById(string id)
        {
            var result = findAllActiveProducts().SingleOrDefault(p => p.PRODUCT_ID == id);
            return result;
        }

        public int findPriceBySizeAndId(int size, string id)
        {
            var product_price_id = _db.ProductDetails.ToList().SingleOrDefault(i => i.Size == size && i.ProductId.Equals(id));
            int result = _db.ProductPrices.ToList().SingleOrDefault(i => i.ProductPriceId.Equals(product_price_id.ProductPriceId)).SalePrice;
            return result;
        }

        public ProductDetail findProductDetailByProductIdAndSize(int size, string product_id)
        {
            ProductDetail product_detail;
            if(size ==0)
            {
                product_detail = _db.ProductDetails.ToList().SingleOrDefault(i => i.ProductId.Equals(product_id));
            }
            else
            {
                product_detail = _db.ProductDetails.ToList().SingleOrDefault(i => i.Size == size && i.ProductId.Equals(product_id));
            }

            // Lay ve nhung truong du lieu can lay
            ProductDetail result = new ProductDetail();
            // Chi ra result co doi tuong product va product price
            // Neu ko se bi loi null object reference khi gan id product va sale price cho result

            result.Product = new Product();
            result.ProductPrice = new ProductPrice();

            result.ProductDetailId = product_detail.ProductDetailId;
            result.Product.Id = product_detail.Product.Id;
            result.Size = product_detail.Size;
            result.Quantity = product_detail.Quantity;
            result.ProductPrice.SalePrice = product_detail.ProductPrice.SalePrice;
            result.Product.Image = product_detail.Product.Image;
            result.Product.Name = product_detail.Product.Name;
            result.Product.GeomancyId = product_detail.Product.GeomancyId;
            result.Product.MainStoneId = product_detail.Product.MainStoneId;

            return result;
        }

        public Product findById(string id)
        {
            return _db.Products.ToList().SingleOrDefault(i => i.Id.Equals(id));
        }

        public ProductDetailDisplay findProductDetailDisplayByProductId(string id)
        {
            ProductDetailDisplay product_detail_display = new ProductDetailDisplay();

            // Tim ten, ma san pham, mainstone, substone, color, thumbnail, images va geomancy

            var product = _db.Products.ToList().SingleOrDefault(i => i.Id.Equals(id));

            product_detail_display.NAME = product.Name;
            product_detail_display.CODE = product.Id;
            product_detail_display.MAIN_STONE = product.MainStone.Name;
            product_detail_display.SUB_STONE = product.SubStone.Name;
            product_detail_display.COLOR = product.Color;
            product_detail_display.THUMB_NAIL = product.Image;
            product_detail_display.IMAGES = product.Images.ToList();
            product_detail_display.GEOMANCY = product.Geomancy.Name;
            product_detail_display.countProduct = product.countProduct;

            var productDetail = _db.ProductDetails.ToList().Where(i => i.ProductId.Equals(id)).OrderBy(s => s.Size).FirstOrDefault();

            // Tim gia cua san pham

            double price = productDetail.ProductPrice.SalePrice;

            // Kiem tra san pham co duoc giam gia

            var isDiscount = new ProductDiscount();

            foreach (var item in _db.ProductDiscounts.ToList())
            {
                if (item.ProductId != null)
                {
                    if (item.ProductId.Equals(product_detail_display.CODE))
                    {
                        isDiscount.DiscountValue = (int)item.DiscountValue;
                        isDiscount.DiscountUnit = item.DiscountUnit;
                    }
                }
                else if (item.GemId != null)
                {
                    if (item.GemId.Equals(product.GeomancyId))
                    {
                        isDiscount.DiscountValue = (int)item.DiscountValue;
                        isDiscount.DiscountUnit = item.DiscountUnit;
                    }
                }
                else if (item.StoneId != null)
                {
                    if (item.GemId.Equals(product.MainStoneId))
                    {
                        isDiscount.DiscountValue = (int)item.DiscountValue;
                        isDiscount.DiscountUnit = item.DiscountUnit;
                    }
                }
            }

            if (isDiscount !=null && isDiscount.DiscountValue != 0)
            {
                //product_detail_display.DISCOUNT_VALUE = (int)isDiscount.DiscountValue;
                product_detail_display.UNIT = isDiscount.DiscountUnit;
                product_detail_display.PRICE_AFTER_DISCOUNT = (price * (double)(100 - product_detail_display.DISCOUNT_VALUE) / (double)100); 
            }

            product_detail_display.PRICE = (int)price;
            return product_detail_display;
        }

        public List<CustomerReview> findAllReviewById(string id)
        {
            var customerReviews = new List<CustomerReview>();

            var reviews = _db.Reviews.ToList().Where(r => r.ProductId.Equals(id)).OrderByDescending(i => i.Created_Date).ToList();

            foreach (var item in reviews)
            {
                var customerReview = new CustomerReview();
                customerReview.CONTENT = item.Content;
                customerReview.CREATED = (DateTime)item.Created_Date;
                customerReview.CUSTOMER_ID = item.CustomerId;
                customerReview.CUSTOMER_NAME = item.Customer.Fullname;
                customerReview.IS_UPDATE = (bool)item.Is_Update;
                customerReview.RATE = (int)item.Rate;
                customerReview.TITLE = item.Title;

                customerReviews.Add(customerReview);
            }

            return customerReviews;
        }

        public List<OverViewProductHomeFlag> findAllProductByCategory(string id)
        {
            var param = new SqlParameter[] {
                        new SqlParameter() {
                            ParameterName = "@cat_id",
                            SqlDbType =  System.Data.SqlDbType.Char,
                            Size = 10,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = id
                        }};
            var result = _db.OverViewProductHomeFlags.FromSqlRaw("[dbo].[sp_findall_product_by_cat_id_with_min_price] @cat_id", param).ToList();

            // Kiem tra san pham het hang neu so luong = 0 thi khong hien thi

            for (int i = 0; i < result.Count; i++)
            {
                if (totalQuantityOfProduct(result[i].PRODUCT_ID) == 0)
                {
                    result.Remove(result[i]);
                }
            }

            return result;
        }

        public List<OverViewProductHomeFlag> findAllActiveProducts()
        {
            var result = _db.OverViewProductHomeFlags.FromSqlRaw("[dbo].[sp_findall_active_product_with_min_price]").ToList();

            var pds = _db.ProductDetails.ToList();

            // Kiem tra san pham het hang neu so luong = 0 thi khong hien thi

            for (int i = 0; i < result.Count; i++)
            {
                if (totalQuantityOfProduct(result[i].PRODUCT_ID) == 0)
                {
                    result.Remove(result[i]);
                }
            }

            return result;
        }

        public List<OverViewProductBestSeller> findAllBestSellerProducts()
        {
            var bestSellerProducts = _db.OverViewProductHomeFlags.FromSqlRaw("[dbo].[sp_findall_best_seller_product_with_min_price]").ToList();

            var result = new List<OverViewProductBestSeller>();

            foreach (var item in bestSellerProducts)
            {
                var product = new OverViewProductBestSeller();
                product.ACTIVE = item.ACTIVE;
                product.DISCOUNT_VALUE = item.DISCOUNT_VALUE;
                product.DIS_NAME = item.DIS_NAME;
                product.IMAGE = item.IMAGE;
                product.IS_SOLDOUT = false;
                product.NAME = item.NAME;
                product.PRICE = item.PRICE;
                product.PRODUCT_ID = item.PRODUCT_ID;
                result.Add(product);
            }

            var pds = _db.ProductDetails.ToList();

            //Kiem tra san pham het hang neu so luong = 0 thi hien thi chay hang

            for (int i = 0; i < result.Count; i++)
            {
                if (totalQuantityOfProduct(result[i].PRODUCT_ID) == 0)
                {
                    result[i].IS_SOLDOUT = true;
                }
            }

            return result;
        }

        public void Update(Product product)
        {
            _db.Products.Update(product);
            _db.SaveChanges();
           
        }
    }
}
