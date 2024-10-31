using btl_web_coffeeshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace btl_web_coffeeshop.Controllers
{
    public class CartController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public CartController(CoffeeShopDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            // Implement logic to get the current user's ID
            return 1; // Placeholder
        }

        public async Task<IActionResult> Cart()
        {
            var userId = GetUserId();
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.Cart.UserId == userId)
                .ToListAsync();

            ViewBag.CartItemCount = cartItems.Count;

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return Json(new { success = false });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == GetUserId());

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = GetUserId(),
                    CreatedDate = DateTime.Now
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = request.ProductId,
                    Quantity = 1,
                    Price = product.Price
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
    }

    public class AddToCartRequest
	{
		public int ProductId { get; set; }
	}
}
