using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.ModelHelpers;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce.CQRS.Handlers.User
{
    public class VerificationHandler : IRequestHandler<VerificationCommand, AuthenticationResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly Token token;
        private readonly IdContext _context;
        public VerificationHandler(UserManager<AppUser> userManager, Token token, IdContext coursesContext)
        {
            _userManager = userManager;
            this.token = token;
            _context = coursesContext;
        }
        public async Task<AuthenticationResponse> Handle(VerificationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return new AuthenticationResponse { Message = "User not found." };

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await _userManager.ConfirmEmailAsync(user, request.VerificationCode);

                if (!result.Succeeded)
                {
                    return new AuthenticationResponse { Message = "Invalid verification code." };
                }

                var tokenString = await token.CreateToken(user);
                var refreshToken = token.GeneratorRefreshToken();

                user.RefreshTokens!.Add(refreshToken);
                await _userManager.UpdateAsync(user);
                await _userManager.AddToRoleAsync(user, "User");

                await transaction.CommitAsync(cancellationToken);

                return new AuthenticationResponse
                {
                    Message = "Email confirmed.",
                    Token = tokenString,
                    RefreshToken = refreshToken.Token,
                    RefreshTokenExpiration = refreshToken.Expires,
                    Roles = (await _userManager.GetRolesAsync(user)).ToList(),
                    IsAuthenticated = true
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new AuthenticationResponse { Message = ex.Message };
            }


        }
    }
}
