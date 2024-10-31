using btl_web_coffeeshop.Models;
using btl_web_coffeeshop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace btl_web_coffeeshop.Controllers
{
    public class HomeController : Controller
    {
		private readonly CoffeeShopDbContext _context;

		public HomeController(CoffeeShopDbContext context)
		{
			_context = context;
		}

		//Home page
		public async Task<IActionResult> Index()
        {
			var categories = await _context.Categories
		   .Include(c => c.Products)
		   .ToListAsync();

			var bestSellerProducts = await _context.Products
								.Where(p => p.Name == "Iced Milk Coffee" ||
											p.Name == "Lychee Hi-Tea" ||
											p.Name == "Butter Croissant" ||
											p.Name == "Cheese Floss")
								.ToListAsync();

			var discoverProducts = await _context.Products
				.OrderByDescending(p => p.CreatedDate)
				.Take(10)
				.ToListAsync();

			var viewModel = new HomeViewModel
			{
				CategoryViewModels = categories.Select(c => new CategoryViewModel
				{
					CategoryId = c.CategoryId,
					Name = c.Name,
					Products = c.Products.ToList()
				}),
				BestSellerProducts = bestSellerProducts,
				DiscoverProducts = discoverProducts
			};

			return View(viewModel);
		}

		//Menu page
		public async Task<IActionResult> Menu()
		{
			var categories = await _context.Categories
				.Include(c => c.Products)
				.ToListAsync();

			var categoryViewModels = categories.Select(c => new CategoryViewModel
			{
				Name = c.Name,
				Products = c.Products.ToList()
			}).ToList();

			return View(categoryViewModels);
		}

		//Services
		public IActionResult Services()
		{
			return View();
		}

		//Blog
		public IActionResult Blog()
        {
            return View();
        }

		//About
		public IActionResult About()
		{
			return View();
		}

        //Shop
        public IActionResult Shop() {
			return View();
		}

		//Contact
		public IActionResult Contact()
		{
			return View();  
		}

        //Cart
        public IActionResult Cart()
        {
            return View();
        }

        //Login
        public IActionResult Login()
        {
            return View();
        }

		//single product
		public IActionResult SingleProduct()
		{
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
