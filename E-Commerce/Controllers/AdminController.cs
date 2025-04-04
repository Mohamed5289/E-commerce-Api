using E_Commerce.CQRS.Commands;
using E_Commerce.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _mediator.Send(new UsersQuery());

            if (users is null)
                return BadRequest(new { Message = "Not Found Users to System" });

            if (users.Count == 0)
                return BadRequest(new { Message = "Not Found Users to System" });

            return Ok(users);
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] string username = "johndoe123")
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { Message = "Username is required" });
            }

            string pattern = "^[a-zA-Z0-9_]+$";
            if (!Regex.IsMatch(username, pattern))
            {
                return BadRequest(new { Message = "Username can only contain letters, numbers, and underscores." });
            }

            if (username.Length < 3 || username.Length > 20)
            {
                return BadRequest(new { Message = "Username must be between 3 and 20 characters." });
            }


            var result = await _mediator.Send(new RemoveUserCommand(username));

            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Message });
            }
            return Ok(result);

        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _mediator.Send(new RoleQuery());

            if (roles is null)
                return BadRequest(new { Message = "Not Found Users to System" });

            if (roles.Count == 0)
                return BadRequest(new { Message = "Not Found Users to System" });

            return Ok(roles);
        }

        [HttpPost("Seed_Role")]
        public async Task<IActionResult> SeedRole(RoleCommand command)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(errors);
            }

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromQuery] RemoveRoleCommand request )
        {
            var result = await _mediator.Send(request);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = result.Message });
            }
            return Ok(result);
        }

    }
}
