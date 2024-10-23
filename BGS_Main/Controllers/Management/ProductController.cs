using Application.DTOs.Request.Management;
using Application.Interfaces.Management;
using Application.Interfaces.Managements;
using Application.ViewModels.Management;

//using Application.ViewModels.Managements;
using Domain.Models;
using Domain.Models.Management;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BGS_Main.Controllers.Managements
{
    public class ProductController : Controller
    {
        private readonly IProductService _product;
        private readonly ICategoryService _category;
        private readonly BGSDbContext _context;
        private readonly IHomeService _home;

        public ProductController(IProductService product, ICategoryService category, BGSDbContext context, IHomeService home)
        {
            _product = product;
            _category = category;
            _context = context;
            _home = home;
        }

        [HttpGet]
        public async Task<IActionResult> Product()
        {
            var products = await _product.GetAllProducts();
            return View(products);
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> ProductPageForUser()
        {
            var products = await _product.GetAllOldOrNewProducts();
            return View(products);
        }

        // [HttpGet]
        // [Route("featured")]
        // public async Task<IActionResult> GetFeaturedProducts()
        // {
        //     var featuredProducts = _context.Products.Where(p => p.IsFeatured).ToList();
        //     return Json(featuredProducts);
        //     //return PartialView("_FeaturedProductsPartial", featuredProducts);            
        // }
        

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ProductDetail(int id, string sterm = "", int categoryId = 0)
        {               
            var productResponse = await _product.GetProductById(id);
            if(productResponse == null)
            {
                return NotFound($"Khong co san pham {productResponse}");
            }

            if (categoryId == 0)
            {
                categoryId = productResponse.CategoryId;
            }

            IEnumerable<Product> products = await _home.GetProducts(sterm, categoryId);
			IEnumerable<Category> categories = await _home.Categories();
            
            var relatedProducts = await _product.GetAllRelatedProducts(id, 5);                        
            // if(!relatedProducts.Any())
            // {
            //     return NotFound("Loi");
            // }
            var viewModel = new RelatedProduct()
            {
                Product = productResponse,
                Products = products, 
                RelatedProducts = relatedProducts, 
                Categories = categories, 
                STerm = sterm, 
                CategoryId = categoryId
            };

            Console.WriteLine($"Product: {viewModel.Product.ProductName}");
            foreach (var rp in viewModel.RelatedProducts)
            {
                Console.WriteLine($"Related Product: {rp.ProductName}");
            }

            return viewModel == null ? NotFound() : View(viewModel);            
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _category.GetAllCategories();
            ViewData["Categories"] = new SelectList(categories, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(CreateProductRequest model)
        {
            var categories = await _category.GetAllCategories();
            ViewData["Categories"] = new SelectList(categories, "Id", "CategoryName");
            await _product.CreateProduct(model);
            return RedirectToAction(nameof(Product));
        }
    }
}
