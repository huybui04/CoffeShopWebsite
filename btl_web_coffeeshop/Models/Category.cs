using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace btl_web_coffeeshop.Models;

public partial class Category
{
    [Key] // Indicates that this property is the primary key
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "The name is required.")]
    [StringLength(100, ErrorMessage = "The name cannot be longer than 100 characters.")]
    public string Name { get; set; } = null!; // Non-nullable property with a default value

    [StringLength(500, ErrorMessage = "The description cannot be longer than 500 characters.")]
    public string? Description { get; set; } // Nullable property

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
