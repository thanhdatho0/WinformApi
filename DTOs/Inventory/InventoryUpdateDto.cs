using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Inventory;

public class InventoryUpdateDto
{
    [Required(ErrorMessage = "ProductId is required.")]
    public int ProductId { get; set; }
    [Required(ErrorMessage = "ColorId is required.")]
    public int ColorId { get; set; }
    [Required(ErrorMessage = "SizeId is required.")]
    public int SizeId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number > 1.")]
    public int Quantity { get; set; }
}