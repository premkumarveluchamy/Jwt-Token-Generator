using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Jwt_Token_Generator.Services
{
    public class JwtService
    {
        private readonly RsaSecurityKey _signingKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(RsaSecurityKey signingKey, string issuer, string audience)
        {
            _signingKey = signingKey;
            _issuer = issuer;
            _audience = audience;
        }

        public string GenerateToken(IEnumerable<Claim> claims, TimeSpan validFor)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(validFor),
                signingCredentials: new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }

}
