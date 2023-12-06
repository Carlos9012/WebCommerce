using ECommerce.API.Models;

namespace ECommerce.API.Data
{
    public interface IDataAccess
    {
        List<ProductCategory> GetProductCategories();
        ProductCategory GetProductCategory(int id);
        Offer GetOffer(int id);
        List<Product> GetProducts(string category, string subcategory, int count);
        Product GetMinProduct(string category, string subcategory);

        Product GetMaxOfferBySubcategory(string category, string subcategory);

        List<Product> GetDistinctProducts(string category);
        Product GetProduct(int id);
        bool InsertUser(User user);
        string IsUserPresent(string emial, string password);
        void InsertReview(Review review);
        List<Review> GetProductReviews(int productId);
        User GetUser(int id);
        bool InsertCartItem(int userid, int productId);
        Cart GetActiveCartOfUser(int userid);
        Cart GetCart(int cartid);
        double CalculateCartTotalValue(int cartid);
        List<Cart> GetAllPreviousCartsOfUser(int userid);
        List<PaymentMethod> GetPaymentMethods();
        int InsertPayment(Payment payment);
        int InsertOrder(Order order);
    }
}
