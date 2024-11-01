using btl_web_coffeeshop.Models;
using btl_web_coffeeshop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace btl_web_coffeeshop.Areas.Admin.Controllers
{
    public class CartController : Controller
    {
        private readonly CoffeeShopDbContext _context;

        public CartController(CoffeeShopDbContext context)
        {
            _context = context;
        }

		//public async Task<IActionResult> Checkout()
		//{
		//	var cartItems = await _context.CartItems
		//		.Include(ci => ci.Product)
		//		.ToListAsync();

		//	var cartViewModel = new CartViewModel
		//	{
		//		CartItems = cartItems.Select(ci => new CartItemViewModel
		//		{
		//			ProductId = ci.ProductId,
		//			Name = ci.Product.Name,
		//			Price = ci.Product.Price,
		//			Quantity = ci.Quantity
		//		}).ToList(),
		//		TotalPrice = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
		//		TotalQuantity = cartItems.Sum(ci => ci.Quantity)
		//	};

		//	return View(cartViewModel);
		//}

		[HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = 1,
                    Price = product.Price
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Cart", "Home");
        }

   //     // Inside the CartController class
   //     [HttpPost]
   //     public async Task<IActionResult> RemoveFromCart(int id)
   //     {
   //         var cartItem = await _context.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == id);
   //         if (cartItem == null)
   //         {
   //             return NotFound();
   //         }

   //         _context.CartItems.Remove(cartItem);
   //         await _context.SaveChangesAsync();

   //         // Tính lại tổng tiền
   //         var cartItems = await _context.CartItems.ToListAsync();
   //         var totalPrice = cartItems.Sum(i => i.Quantity * i.Price);

   //         var cartViewModel = new CartViewModel
   //         {
   //             CartItems = cartItems,
   //             TotalPrice = totalPrice
			//};

   //         return PartialView("_CartPartial", cartViewModel);
   //     }
    }
}
