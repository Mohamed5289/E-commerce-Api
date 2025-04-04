using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.ModelHelpers;
using MediatR;

namespace E_Commerce.CQRS.Handlers.Product
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, GeneralDTO>
    {
        private readonly IdContext idContext;
        private readonly ImageService imageService;

        public DeleteProductHandler(IdContext idContext , ImageService imageService)
        {
            this.idContext = idContext;
            this.imageService = imageService;

        }
        public async Task<GeneralDTO> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = idContext.Products.FirstOrDefault(x => x.Name == request.Name);

            if (product == null)
            {
                return new GeneralDTO
                {
                    IsSuccess = false,
                    Message = "ProductName not found."
                };
            }

            var image = product.ImageUrl.Split("/").Last();

            imageService.DeleteImage(image , "wwwroot", "image");

            idContext.Products.Remove(product);
            await idContext.SaveChangesAsync();
            return new GeneralDTO
            {
                IsSuccess = true,
                Message = "ProductName deleted successfully."
            };
        }


    }
}
