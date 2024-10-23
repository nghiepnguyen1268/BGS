using Domain.Models.Auth;
using Domain.Models.Management;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BGSDbContext : IdentityDbContext<User>
    {
        public BGSDbContext(DbContextOptions<BGSDbContext> options) : base(options) {}

        // Set the tables to the database
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; } 
        public DbSet<CartDetails> CartDetails { get; set; } 
        public DbSet<Order> Orders { get; set; } 
        public DbSet<OrderDetails> OrderDetails { get; set; } 
        public DbSet<OrderStatus> OrderStatus { get; set; } 
        public DbSet<Stock> Stocks { get; set; }
    }
}