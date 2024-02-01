using System.ComponentModel.DataAnnotations;

namespace ShopAppAPI.Models;

public class Category {
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public virtual List<Product> Products { get; set; }

}