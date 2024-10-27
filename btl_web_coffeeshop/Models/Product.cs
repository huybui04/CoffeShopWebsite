using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace btl_web_coffeeshop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public int? Stock { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? CreatedDate { get; set; }

    [NotMapped] // Đảm bảo không lưu vào DB
    public IFormFile? UploadedImage { get; set; }
    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
}
