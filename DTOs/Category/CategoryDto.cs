

using api.DTOs.Subcategory;

namespace api.DTOs.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TargetCustomerId { get; set; }
        public List<SubcategoryDto>? Subcategories { get; set; }
    }
}