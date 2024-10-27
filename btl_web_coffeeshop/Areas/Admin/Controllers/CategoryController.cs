using btl_web_coffeeshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace btl_web_coffeeshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Category")]
    public class CategoryController : Controller
    {
        CoffeeShopDbContext db = new CoffeeShopDbContext();

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [Route("Edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = db.Categories.Find(id); // Find category by ID
            if (category == null)
            {
                return NotFound(); // Return 404 if not found
            }
            return View(category);
        }

        [Route("Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest(); // Return 400 if IDs do not match
            }

            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified; // Update the entity
                db.SaveChanges();
                return RedirectToAction("Index"); // Redirect to Index after editing
            }
            return View(category); // Return to the view with model state errors
        }

        [Route("Delete/{id}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound(); // Nếu không tìm thấy danh mục
            }

            // Kiểm tra xem có sản phẩm nào liên quan không
            if (HasProducts(id))
            {
                TempData["ErrorMessage"] = "Không thể xóa danh mục này vì nó có sản phẩm liên quan.";
                return RedirectToAction("Index"); // Chuyển hướng về danh sách
            }

            return View(category); // Trả về view với danh mục cần xóa
        }


        [Route("Delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound(); // Nếu không tìm thấy danh mục
            }


            // Nếu không có sản phẩm liên quan, xóa danh mục
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index"); // Chuyển hướng về danh sách
        }

        private bool HasProducts(int categoryId)
        {
            return db.Products.Any(p => p.CategoryId == categoryId);
        }

    }
}