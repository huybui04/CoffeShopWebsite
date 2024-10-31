using btl_web_coffeeshop.Models;

namespace btl_web_coffeeshop.ViewModels
{
	public class CategoryViewModel
	{
		public int CategoryId { get; set; }
		public string Name { get; set; }
		public List<Product> Products { get; set; }
	}
}
