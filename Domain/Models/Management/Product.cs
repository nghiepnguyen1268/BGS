using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models.Auth;

namespace Domain.Models.Management
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? Pictures { get; set; }
        public double ProductPrice { get; set; } 
        public bool IsFeatured { get; set; } = false;
        public double? DiscountProductprice { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Relationship 
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
        public List<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>(); 
        public List<CartDetails> CartDetails { get; set; } = new List<CartDetails>();       
        public Stock? Stock { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
    }
}