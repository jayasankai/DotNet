namespace TestAPI.Models;

using System.Text.Json.Serialization;

public class Product {
    public int Id { get; set; }

    public string Name { get; set; }  = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Sku { get; set; }

    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }

    public int CategoryId { get; set; }

    [JsonIgnore]
    public virtual Category Category { get; set; }

}