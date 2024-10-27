using btl_web_coffeeshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace btl_web_coffeeshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Route("Admin/HomeAdmin")]
    public class HomeAdminController : Controller
    {
        CoffeeShopDbContext db = new CoffeeShopDbContext();

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}