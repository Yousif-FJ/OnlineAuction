using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineAuctionBackend.Identity.Services
{
    public interface IAccessTokenGenerator
    {
        public string GenerateAccessToken(string userName, string email, string id);
    }

    internal class AccessTokenGenerator : IAccessTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public AccessTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateAccessToken(string username, string email, string id)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            var secret = _configuration["AUCTION_JWT_SECRET"];
            if (secret is null)
            {
                throw new KeyNotFoundException("Faild to obtain JWT Secret");
            }

            var key = Encoding.UTF8.GetBytes(secret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims: new[]
                {
                    new Claim(type: JwtRegisteredClaimNames.UniqueName, username),
                    new Claim(type: JwtRegisteredClaimNames.Email, email),
                    new Claim(type: JwtRegisteredClaimNames.NameId, id),
                    new Claim(type: JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:ExpiresInDay"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"]
            };
            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
