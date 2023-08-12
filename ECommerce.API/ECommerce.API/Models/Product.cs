namespace ECommerce.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string description_ { get; set; } = string.Empty;
        public ProductCategory ProductCategory { get; set; } = new ProductCategory();
        public Offer offer { get; set; } = new Offer();
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageName { get; set; } = string.Empty;
    }
}
