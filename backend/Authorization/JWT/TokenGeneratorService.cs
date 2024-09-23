using Authorization.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authorization.JWT
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly AuthOptions _authSettings;

        public TokenGeneratorService(IOptions<AuthOptions> authSettings)
        {
            _authSettings = authSettings.Value;
        }

        public string GenerateJwtToken(User user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Add role claim based on user email or username
            if (user.Email == "admin@example.com" || user.UserName == "Admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            // Define the security key and credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create and return the JWT token
            var jwt = new JwtSecurityToken(
            issuer: _authSettings.Issuer,
            audience: _authSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
