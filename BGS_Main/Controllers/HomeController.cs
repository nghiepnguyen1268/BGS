using Application.Interfaces.Management;
using Application.ViewModels.Management;
using BGS_Main.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BGS_Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _home;

        public HomeController(ILogger<HomeController> logger, IHomeService home)
        {
            _logger = logger;
            _home = home;
        }

        public async Task<IActionResult> Index()
        {
            var featuredProducts = await _home.GetFeaturedProducts(5);
            if(!featuredProducts.Any())
            {
                return NotFound("Loi related products");
            }

            var viewModel = new IndexProduct()
            {
                FeaturedProduct = featuredProducts
            };

            foreach (var rp in viewModel.FeaturedProduct)
            {
                Console.WriteLine($"Related Product: {rp.ProductName}");
            }
            
            return viewModel == null ? NotFound() : View(viewModel); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
