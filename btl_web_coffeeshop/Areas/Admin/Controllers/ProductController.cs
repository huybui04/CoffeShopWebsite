﻿using btl_web_coffeeshop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace btl_web_coffeeshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product")]
    public class ProductController : Controller
    {
        CoffeeShopDbContext db = new CoffeeShopDbContext();
        private readonly IWebHostEnvironment _environment;

        public ProductController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu có tệp tải lên
                if (product.UploadedImage != null)
                {
                    // Tạo tên file ngẫu nhiên để tránh trùng lặp
                    var fileName = Path.GetFileNameWithoutExtension(product.UploadedImage.FileName);
                    var extension = Path.GetExtension(product.UploadedImage.FileName);
                    var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    // Đường dẫn đến thư mục wwwroot/img/products
                    var path = Path.Combine(_environment.WebRootPath, "img", "products", newFileName);

                    // Lưu tệp
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await product.UploadedImage.CopyToAsync(stream);
                    }

                    // Lưu tên và URL vào DB
                    product.ImageUrl = $"/img/products/{newFileName}";
                }

                product.CreatedDate = DateTime.Now; // Thêm ngày tạo nếu cần

                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); // or log it
                }
                return View(product); // Return the view with the product model to show validation errors
            }

            // Nếu có lỗi, nạp lại danh sách danh mục
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }


        [Route("Details")]
        [HttpGet]
        public IActionResult Details(int id)
        {
            // Tìm sản phẩm dựa trên id
            var product = db.Products
                .Include(p => p.Category) // Bao gồm category nếu bạn cần hiển thị tên category
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy sản phẩm
            }

            return View(product); // Trả về view với sản phẩm
        }

        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await db.Products.FindAsync(product.ProductId);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin sản phẩm
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.CategoryId = product.CategoryId;

                // Kiểm tra nếu có tệp tải lên
                if (product.UploadedImage != null)
                {
                    // Tạo tên file ngẫu nhiên để tránh trùng lặp
                    var fileName = Path.GetFileNameWithoutExtension(product.UploadedImage.FileName);
                    var extension = Path.GetExtension(product.UploadedImage.FileName);
                    var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";

                    // Đường dẫn đến thư mục wwwroot/img/products
                    var path = Path.Combine(_environment.WebRootPath, "img", "products", newFileName);

                    // Lưu tệp
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await product.UploadedImage.CopyToAsync(stream);
                    }

                    // Cập nhật URL hình ảnh
                    existingProduct.ImageUrl = $"/img/products/{newFileName}";
                }

                // Cập nhật ngày tạo nếu cần
                existingProduct.CreatedDate = DateTime.Now;

                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, nạp lại danh sách danh mục
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = db.Products.Include(p => p.Category).FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product); // Trả về view xác nhận xóa sản phẩm
        }

        [HttpPost]
        [Route("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Đường dẫn đến ảnh
            var imagePath = Path.Combine(_environment.WebRootPath, product.ImageUrl.TrimStart('/'));

            // Xóa ảnh nếu tồn tại
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            // Xóa sản phẩm
            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); // Chuyển hướng về danh sách sản phẩm
        }


    }
}