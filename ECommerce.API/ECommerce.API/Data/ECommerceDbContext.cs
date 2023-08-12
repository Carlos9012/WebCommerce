using ECommerce.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ECommerce.API.Data
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions options) : base(options) 
        {   
        }

        public DbSet<Offer> offers { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Review> reviews { get; set; }
    }
}
