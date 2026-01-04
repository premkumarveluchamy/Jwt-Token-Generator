using Jwt_Token_Generator.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IO;

namespace Jwt_Token_Generator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpGet(Name = "GetJWT")]
        public IActionResult Login()
        {
            // Load private key
            string privateKeyPem = System.IO.File.ReadAllText("Keys/private_key_pkcs8.pem");
            using RSA rsa = RSA.Create();
            rsa.ImportFromPem(privateKeyPem);
            var signingKey = new RsaSecurityKey(rsa);

            // Create claims
            var claims = new List<Claim>
                  {
                        new Claim(JwtRegisteredClaimNames.Sub, "user123"), // Registered claim
                        new Claim(JwtRegisteredClaimNames.UniqueName, "premkumar"),
                        new Claim("role", "admin"),                        // Public claim
                        new Claim("custom_claim", "custom_value")          // Private claim
                  };

            // Generate token
            var jwtService = new JwtService(signingKey, "https://your-issuer.com", "your-audience");
            string token = jwtService.GenerateToken(claims, TimeSpan.FromHours(1));
            return Ok(new { token });
        }
    }
}
