using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Product;

public class ProductInfoDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FirstPicture { get; set; } = string.Empty;
    public string? Alt { get; set; } = string.Empty;
}