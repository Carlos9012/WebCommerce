namespace ECommerce.API.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        public Product Product { get; set; } = new Product();
    }
}
