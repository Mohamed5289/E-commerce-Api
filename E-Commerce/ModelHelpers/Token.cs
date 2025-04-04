using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.ModelHelpers
{
    public class Token
    {
        private readonly UserManager<AppUser> userManager;
        private readonly Jwt jwt;

        public Token(UserManager<AppUser> userManager, IOptions<Jwt> options)
        {
            this.userManager = userManager;
            jwt = options.Value;
        }
        public async Task<string> CreateToken(AppUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);


            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            claims.AddRange(userRoles.Select(role => new Claim("role", role)));
            claims.AddRange(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(jwt.ExpireOfDay),
                SigningCredentials = creds,
                Issuer = jwt.Issuer,
                Audience = jwt.Audience
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        public RefreshToken GeneratorRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                Expires = DateTime.UtcNow.AddDays(jwt.ExpireOfDay),
                Created = DateTime.UtcNow
            };

        }
    }
}
