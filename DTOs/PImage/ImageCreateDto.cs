using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace api.DTOs.PImage
{
    public class ImageCreateDto
    {

        [Required(ErrorMessage = "ProductId is required.")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "ColorId is required.")]
        public int ColorId { get; set; }

        [Required(ErrorMessage = "Url is required.")]
        [DefaultValue("")]
        public string Url { get; set; } = string.Empty;
        // [Required(ErrorMessage = "Alt is required.")]
        [DefaultValue("")]
        [StringLength(100, ErrorMessage = "Alt text cannot exceed 100 characters.")]
        public string? Alt { get; set; }
    }
}