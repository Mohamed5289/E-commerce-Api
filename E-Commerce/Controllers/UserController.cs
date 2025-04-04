using E_Commerce.CQRS.Commands;
using E_Commerce.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace E_Commerce.Controllers
{
	[EnableRateLimiting("UserLimiter")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ModelHelpers.IMailService mailService;

        public UserController(IMediator mediator, ModelHelpers.IMailService mailService)
        {
            _mediator = mediator;
            this.mailService = mailService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterCommand registerCommand)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return BadRequest(errors);
            }

            var result = await _mediator.Send(registerCommand);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = result.Message });
            }

            var message = await mailService.SendEmail(result.Email, "Email Confirmation", $"Please confirm your email by clicking <a href=$'{Request.Scheme}://{Request.Host}/api/User/VerifyEmail?Email={result.Email}&verificationCode={result.VerificationCode}'>here</a>");


            return Ok(new
            {
                Message = result.Message
            });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand loginCommand)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(errors);
            }
            var result = await _mediator.Send(loginCommand);

            if (!result.IsAuthenticated)
            {
                return BadRequest(new { Message = result.Message });
            }

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Lax
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        [HttpGet("Refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken)) return BadRequest(new { Message = "Invalid client request" });


            var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));

            if (!result.IsAuthenticated) return BadRequest(new { Message = result.Message });

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(new { Message = result });
        }

        [HttpGet("Revoke")]
        public async Task<IActionResult> RevokeToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Invalid client request");

            var result = await _mediator.Send(new RevokeTokenCommand(refreshToken));

            if (!result) return BadRequest(new { Message = "Invalid client request" });

            return Ok(new { Message = "Token revoked" });
        }

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail( [FromQuery] VerificationCommand verificationCommand)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(errors);
            }

            var result = await _mediator.Send(verificationCommand);

            if (!result.IsAuthenticated)
            {
                return BadRequest(new { Message = result.Message });
            }

            return Ok(result);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand forgetPasswordCommand)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(errors);
            }
            var result = await _mediator.Send(forgetPasswordCommand);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = result.Message });
            }
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> RestPassword(ResetPasswordCommand restPasswordCommand)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );
                return BadRequest(errors);
            }
            var result = await _mediator.Send(restPasswordCommand);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = result.Message });
            }
            return Ok(result);
        }

        [HttpGet("GetUserByUserName/{username}")]

        public async Task<IActionResult> GetUserByUserName([FromRoute] string username = "johndoe123")
        {
            if(string.IsNullOrEmpty(username))
            {
                return BadRequest(new { Message = "Username is required." });
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

            var user = await _mediator.Send(new UserByUsernameQuery(username));

            if (user is null)
                return BadRequest(new { Message = "User is not found or UserName is not correct" });

            return Ok(user);

        }

    }
}
