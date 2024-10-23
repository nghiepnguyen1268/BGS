using Domain.Models.Management;

namespace Application.ViewModels.Management
{
    public class IndexProduct
    {   
        //public Product Products { get; set; }
        public IEnumerable<Product> FeaturedProduct { get; set; }
    }
}