using Microsoft.AspNetCore.Mvc;
using btl_web_coffeeshop.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace btl_web_coffeeshop.Components
{
    public class CartItemCountViewComponent : ViewComponent
    {
        private readonly CoffeeShopDbContext _context;

        public CartItemCountViewComponent(CoffeeShopDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cartItemCount = await _context.CartItems.SumAsync(item => item.Quantity);
            return View(cartItemCount);
        }
    }
}