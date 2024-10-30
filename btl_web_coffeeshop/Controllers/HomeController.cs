using btl_web_coffeeshop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace btl_web_coffeeshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Home page
        public IActionResult Index()
        {
            return View();
        }

        //Menu page
        public IActionResult Menu()
		{
			return View();
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

		//checkout
		public IActionResult Checkout()
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
