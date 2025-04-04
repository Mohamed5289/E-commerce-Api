using E_Commerce.CQRS.Commands;
using E_Commerce.Data;
using E_Commerce.ModelDTOs;
using E_Commerce.ModelHelpers;
using E_Commerce.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.CQRS.Handlers.User
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
    {
        private readonly IdContext _context;
        private readonly Token token;
        private readonly UserManager<AppUser> userManager;

        public LoginHandler(IdContext context, Token token, UserManager<AppUser> userManager)
        {
            _context = context;
            this.token = token;
            this.userManager = userManager;
        }
        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Username);

                if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                {
                    return new AuthenticationResponse { Message = "Error in Username or Password" };
                }

                var isConfirmed = await userManager.IsEmailConfirmedAsync(user);

                if (!isConfirmed)
                {
                    return new AuthenticationResponse { Message = "Email not confirmed." };
                }

                var tokenString = await token.CreateToken(user);

                if (user.RefreshTokens!.Any(u => u.IsActive))
                {
                    var refreshToken = user.RefreshTokens!.FirstOrDefault(r => r.IsActive);
                    await transaction.CommitAsync(cancellationToken);
                    return new AuthenticationResponse
                    {
                        IsAuthenticated = true,
                        Token = tokenString,
                        RefreshToken = refreshToken!.Token,
                        RefreshTokenExpiration = refreshToken.Expires,
                        Roles = (await userManager.GetRolesAsync(user)).ToList()
                    };
                }
                else
                {

                    var refreshToken = token.GeneratorRefreshToken();
                    user.RefreshTokens!.Add(refreshToken);
                    await userManager.UpdateAsync(user);
                    await transaction.CommitAsync(cancellationToken);
                    return new AuthenticationResponse
                    {
                        IsAuthenticated = true,
                        Token = tokenString,
                        RefreshToken = refreshToken.Token,
                        RefreshTokenExpiration = refreshToken.Expires,
                        Roles = (await userManager.GetRolesAsync(user)).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new AuthenticationResponse { Message = ex.Message };
            }
        }
    }
}
