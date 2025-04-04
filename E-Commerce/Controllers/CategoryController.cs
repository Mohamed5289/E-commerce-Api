using E_Commerce.CQRS.Commands;
using E_Commerce.CQRS.Queries;
using E_Commerce.Migrations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mediator.Send(new CategoriesQuery());

            if (categories is null)
                return BadRequest(new { Message = "Not Found Categories to System" });

            if (categories.Count == 0)
                return BadRequest(new { Message = "Not Found Categories to System" });

            return Ok(categories);
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryCommand request)
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
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }


        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryCommand request)
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
                return BadRequest(new { Message = result.Message });
            }
            return Ok(result);
        }
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryCommand request)
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
                return BadRequest(new { Message = result.Message });
            }
            return Ok(result);
        }
    }
}
