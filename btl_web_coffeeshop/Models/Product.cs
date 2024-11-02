using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; // Ensure you have this for IFormFile

namespace btl_web_coffeeshop.Models;

public partial class Product
{
    [Key] // Marks this property as the primary key
    public int ProductId { get; set; }

    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
    public decimal? Discount { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int? Stock { get; set; }

    [Url(ErrorMessage = "Invalid image URL.")]
    public string? ImageUrl { get; set; }

    public DateTime? CreatedDate { get; set; }

    [NotMapped] // Ensure this property is not mapped to the database
    [Display(Name = "Upload Image")]
    public IFormFile? UploadedImage { get; set; }

    public virtual ICollection<CartItem> CartItems { get; } = new List<CartItem>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
}
