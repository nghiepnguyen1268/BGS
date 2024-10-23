using Domain.Models.Management;

namespace Application.ViewModels.Management
{
    public class RelatedProduct
    {
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Product> RelatedProducts { get; set; }
        public IEnumerable<Category> Categories { get; set; }
		public string STerm { get; set; } = "";
		public int CategoryId { get; set; } = 0;
    }
}
