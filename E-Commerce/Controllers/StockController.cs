using E_Commerce.CQRS.Commands;
using E_Commerce.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Stocks")]
        public async Task<IActionResult> GetStocks()
        {
            var stocks = await _mediator.Send(new StocksQuery());
            if (stocks is null)
                return BadRequest(new { Message = "Not Found Stocks to System" });
            
            if (stocks.Count == 0)
                return BadRequest(new { Message = "Not Found Stocks to System" });
            
            return Ok(stocks);
        }

        [HttpPost("AddStock")]
        public async Task<IActionResult> AddStock(StockCommand request)
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

        [HttpDelete("DeleteStock")]
        public async Task<IActionResult> DeleteStock(DeleteStockCommand request)
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

        [HttpPut("UpdateStock")]
        public async Task<IActionResult> UpdateStock(UpdateStockCommand request)
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
