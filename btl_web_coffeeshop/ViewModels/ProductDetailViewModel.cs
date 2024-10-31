using btl_web_coffeeshop.Models;

namespace btl_web_coffeeshop.ViewModels
{
	public class ProductDetailViewModel
	{
		public Product Product { get; set; }
		public List<Product> RelatedProducts { get; set; }
	}
}
