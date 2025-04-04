using E_Commerce.CQRS.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Commerce.Controllers
{
	[EnableRateLimiting("UserLimiter")]
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SupplierController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("AddSupplier")]
        public async Task<IActionResult> AddSupplier([FromBody] SupplierCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("GetSupplier")]
        public async Task<IActionResult> GetSupplier([FromQuery] SupplierByUsernameCommand request)
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

            if (result is not null)
            {
                return Ok(result);
            }

            return NotFound( new
            {
                Message = "Supplier not found."
            });
        }

        [HttpGet("GetSuppliers")]
        public async Task<IActionResult> GetSuppliers()
        {
            var result = await _mediator.Send(new SuppliersCommand());

            if (result is not null)
            {
                return Ok(result);
            }
            return NotFound(new
            {
                Message = "No suppliers found."
            });
        }

        [HttpPost("AddSupplierProduct")]
        public async Task<IActionResult> AddSupplierProduct([FromBody] SupplierProductCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
