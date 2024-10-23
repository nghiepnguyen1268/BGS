using Domain.Models.Management;

namespace Application.Interfaces.Management
{
    public interface IHomeService
    {
        Task<IEnumerable<Product>> GetAllproducts();
        Task<IEnumerable<Product>> GetFeaturedProducts(int count);
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int categoryId = 0);
        Task<IEnumerable<Category>> Categories();
    }
}