using btl_web_coffeeshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace btl_web_coffeeshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Policy = "AdminOnly")]
    public class HomeAdminController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        // Constructor để nhận CoffeeShopDbContext thông qua Dependency Injection
        public HomeAdminController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
