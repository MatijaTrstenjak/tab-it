using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using tab_it.Models;
using tab_it.Repositories.Contracts;

namespace tab_it.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;

        public HomeController(
            IProductCategoryRepository productCategoryRepository,
            IProductRepository productRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Operations Dashboard";
            return View();
        }

        public IActionResult POS()
        {
            var categories = _productCategoryRepository.GetAll();
            var products = _productRepository.GetAll();

            ViewData["Categories"] = categories;
            ViewData["Products"] = products;

            ViewData["Title"] = "Point of Sale";
            return View();
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
