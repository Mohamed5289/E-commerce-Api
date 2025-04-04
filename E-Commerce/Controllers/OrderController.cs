using E_Commerce.CQRS.Commands;
using E_Commerce.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
	[EnableRateLimiting("UserLimiter")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;
        public OrderController( IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCommand orderCommand)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(errors);
            }

            var result = await mediator.Send(orderCommand);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await mediator.Send(new OrderQuery());

            if (!orders.Any())
            {
                return BadRequest(new
                {
                    Message = "No orders found"
                });
            }
            return Ok(orders);
        }

        [HttpGet("UserOrders")]
        public async Task<IActionResult> UserOrders([FromQuery]UserOrdersCommand request)
        {
            var orders = await mediator.Send(request);

            if (!orders.Any())
            {
                return BadRequest(new
                {
                    Message = "No orders found or User is not found"
                });
            }
            return Ok(orders);
        }

        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder([FromBody] DeleteOrderCommand request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(errors);
            }

            var result = await mediator.Send(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
