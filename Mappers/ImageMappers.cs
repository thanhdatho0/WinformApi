using api.DTOs.PImage;
using api.Models;

namespace api.Mappers
{
    public static class ImageMappers
    {
        public static ImageDto ToImageDto(this Image imageModel)
        {
            return new ImageDto
            {
                ImageId = imageModel.ImageId,
                Url = imageModel.Url,
                Alt = imageModel.Alt!,
                ProductId = imageModel.ProductId,
                ColorId = imageModel.ColorId
            };
        }

        public static Image ToImageFromCreateDto(this ImageCreateDto imageDto)
        {
            return new Image
            {
                Alt = imageDto.Alt,
                ProductId = imageDto.ProductId,
                ColorId = imageDto.ColorId
            };
        }

        public static Image ToImageFromCreateProductDto(this ImageCreateToProductDto imageDto)
        {
            return new Image
            {
                Url = imageDto.Url,
                Alt = imageDto.Alt,
            };
        }
            

        public static void ToProductFromUpdateDto(this Image image, ImageUpdateDto imageDto)
        {
            image.Url = imageDto.Url;
            image.Alt = imageDto.Alt;
        }
    }
}