using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicCommerce.Areas.Customer.Models;
using ElectronicCommerce.Models;

namespace ElectronicCommerce.Areas.Customer.Services
{
    public interface IProductService
    {
        // Tim cac san pham hien o muc home-flag
        public List<OverViewProductHomeFlag> findAllHomeFlagProducts();

        // Tim cac san pham hien o muc active-product
        public List<OverViewProductHomeFlag> findAllActiveProducts();

        // Tim cac san pham hien o muc best-seller-product
        public List<OverViewProductBestSeller> findAllBestSellerProducts();
        public void Update(Product payload);
        public void Save();
        // Tim san pham theo id
        public OverViewProductHomeFlag findProductById(string id);

        // Tim san pham theo id
        public Product findById(string id);

        // Tim cac size hien co cua san pham
        public List<int> findAllSizeOfProducts(string id);

        // Tim gia cua san pham qua size va masp
        public int findPriceBySizeAndId(int size, string id);

        // Tim ve product_detail dua tren product_id va size
        public ProductDetail findProductDetailByProductIdAndSize(int size, string product_id);

        // Tim ve chi tiet san pham theo product_id
        public ProductDetailDisplay findProductDetailDisplayByProductId(string id);
        
        // Tim cac review ve san pham theo product_id
        public List<CustomerReview> findAllReviewById(string id);

        // Tim ve danh sach san pham theo category id
        public List<OverViewProductHomeFlag> findAllProductByCategory(string id);
    }
}
