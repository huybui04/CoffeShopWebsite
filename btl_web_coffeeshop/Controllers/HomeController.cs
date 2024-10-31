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
		//[Route("")]
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

			var viewModel = new HomeViewModel
			{
				CategoryViewModels = categories.Select(c => new CategoryViewModel
				{
					CategoryId = c.CategoryId,
					Name = c.Name,
					Products = c.Products.Take(3).ToList()
				}).ToList(),
				BestSellerProducts = bestSellerProducts,
			};

			return View(viewModel);
		}

		//Menu page
		[Route("Menu")]
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
		[Route("Services")]
		public IActionResult Services()
		{
			return View();
		}

		//Blog
		[Route("Blog")]
		public IActionResult Blog()
        {
            return View();
        }

		//About
		[Route("About")]
		public IActionResult About()
		{
			return View();
		}

		//Contact
		[Route("Contact")]
		public IActionResult Contact()
		{
			return View();  
		}

		//Cart
		[Route("Cart")]
        public IActionResult Cart()
        {
            return View();
        }

        //Login
        public IActionResult Login()
        {
            return View();
        }

		//Product Details
		[Route("Detail/ProductDetail/{id}")]
		public async Task<IActionResult> SingleProduct(int id)
		{
			var product = await _context.Products
				.Include(p => p.Category)
				.FirstOrDefaultAsync(p => p.ProductId == id);

			if (product == null)
			{
				return NotFound();
			}

			var relatedProducts = await _context.Products
								.Where(p => p.CategoryId == product.CategoryId && p.ProductId != id)
								.Take(4)
								.ToListAsync();

			var viewModel = new ProductDetailViewModel
			{
				Product = product,
				RelatedProducts = relatedProducts
			};

			return View(viewModel);
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
