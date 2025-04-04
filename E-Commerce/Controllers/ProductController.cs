using E_Commerce.CQRS.Commands;
using E_Commerce.CQRS.Queries;
using E_Commerce.ModelDTOs;
using E_Commerce.ModelHelpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ImageSetting imageSetting;

        public ProductController(IMediator mediator , IOptions<ImageSetting> imageSetting)
        {
            _mediator = mediator;
            this.imageSetting = imageSetting.Value;
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] AddProductCommand command)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(errors);
            }

            var imageUrl = Upload(command.Image);
            var product = new ProductCommand
            {
                Name = command.Name,
                Price = command.Price,
                Description = command.Description,
                ImageUrl = imageUrl.Url,
                Category = command.Category
            };

            var result = await _mediator.Send(product);

            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        private UploadDTO Upload(IFormFile file)
        {
            #region ValidationExtensions
            var extension = Path.GetExtension(file.FileName);

            //TODO: extension validation in appsettings.json file this best practice


            var isAllowedExtension = imageSetting.AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);

            if (!isAllowedExtension)
            {
                return new UploadDTO
                {
                    IsSuccess = false,
                    Message = "Invalid file extension"
                };
            }
            #endregion

            #region ValidationSize
            var isSizeValid = file.Length is > 0 && file.Length < imageSetting.MaxSize;
            if (!isSizeValid)
            {
                return new UploadDTO
                {
                    IsSuccess = false,
                    Message = "Size is not allowed"
                };
            }
            #endregion

            #region StoreFile
            var fileName = $"{Guid.NewGuid()}{extension}";
            var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" , "image");

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            var path = Path.Combine(imagesPath, fileName);

            using var stream = new FileStream(path, FileMode.Create);

            file.CopyTo(stream);
            #endregion

            #region GenerateUrl
            var upload = new UploadDTO
            {
                IsSuccess = true,
                Url = $"{Request.Scheme}://{Request.Host}/wwwroot/image/{fileName}"
            };

            return upload;
            #endregion

        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromQuery] DeleteProductCommand request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(errors);
            }


            var result = await _mediator.Send(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts()
        {

            var products = await _mediator.Send(new ProductsQuery());
            if (products == null)
            {
                return BadRequest(new
                {
                    Message = "No products found"
                });
            }

            if(products.Count == 0)
            {
                return BadRequest(new
                {
                    Message = "No products found"
                });
            }

            return Ok(products);
        }

        [HttpGet("ProductName")]
        public async Task<IActionResult> GetProduct([FromQuery] ProductQuery request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(errors);

            }
            var product = await _mediator.Send(request);

            if (product == null)
            {
                return BadRequest(new
                {
                    Message = "ProductName not found"
                });
            }
            return Ok(product);
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProduct request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(errors);
            }

            string? imageUrl = null;

            if (request.Image is not null)
            {
               imageUrl = Upload(request.Image).Url;
            }

            var product = new UpdateProductCommand
            {
                Name = request.Name,
                NewName = request.NameNew,
                Price = request.Price,
                Description = request.Description,
                ImageUrl = imageUrl,
                Category = request.Category
            };

            var result = await _mediator.Send(product);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }

}