using btl_web_coffeeshop.Models;

namespace btl_web_coffeeshop.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CartItem> CartItems { get; set; }
        public IEnumerable<Product> RelatedProducts { get; set; }
        public decimal TotalPrice { get; internal set; }
        public int TotalQuantity { get; internal set; }
	}
}
