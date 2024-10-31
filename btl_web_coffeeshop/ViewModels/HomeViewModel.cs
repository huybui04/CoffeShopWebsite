using btl_web_coffeeshop.Models;

namespace btl_web_coffeeshop.ViewModels
{
	public class HomeViewModel
	{
		public IEnumerable<CategoryViewModel> CategoryViewModels { get; set; }
		public List<Product> BestSellerProducts { get; set; }
		public IEnumerable<Product> DiscoverProducts { get; set; }
	}
}
