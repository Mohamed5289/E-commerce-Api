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
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResponse>
    {
        private IdContext _context;
        private Token token;
        private UserManager<AppUser> userManager;

        public RefreshTokenHandler(IdContext context, Token token, UserManager<AppUser> userManager)
        {
            _context = context;
            this.token = token;
            this.userManager = userManager;
        }
        public async Task<AuthenticationResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens!.Any(t => t.Token == request.Token));

                if (user is null) return new AuthenticationResponse { Message = "Token not found" };

                var refreshToken = user.RefreshTokens!.Single(x => x.Token == request.Token);

                if (!refreshToken.IsActive) return new AuthenticationResponse { Message = "Token is not active" };

                refreshToken.Revoked = DateTime.UtcNow;

                var newRefreshToken = token.GeneratorRefreshToken();
                user.RefreshTokens!.Add(newRefreshToken);
                await userManager.UpdateAsync(user);

                var jwtToken = await token.CreateToken(user);


                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return new AuthenticationResponse
                {
                    IsAuthenticated = true,
                    Token = jwtToken,
                    RefreshToken = newRefreshToken.Token,
                    RefreshTokenExpiration = newRefreshToken.Expires,
                    Roles = (await userManager.GetRolesAsync(user)).ToList(),
                    Message = "Token revoked"
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
