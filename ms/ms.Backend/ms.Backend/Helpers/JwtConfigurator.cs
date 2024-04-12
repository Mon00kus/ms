using Microsoft.IdentityModel.Tokens;
using ms.Backend.Domain.Models;
using ms.Backend.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ms.Backend.Helpers
{
    public class JwtConfigurator
    {
        public static string GetToken(UserLogin userInfo, IConfiguration config)
        {
            string SecretKey = config["JwtBearer:SecretKey"]!;
            string Issuer = config["JwtBearer:Issuer"]!;
            string Audience = config["JwtBearer:Audience"]!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim("idUsuario", userInfo.Id.ToString())
            };

            var token = new JwtSecurityToken(
               issuer: Issuer,
               audience: Audience,
               claims,
               expires: DateTime.Now.AddDays(1),
               signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static int GetTokenIdUsuario(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                foreach (var claim in claims)
                {
                    if (claim.Type == "idUsuario")
                    {
                        return int.Parse(claim.Value);
                    }
                }
            }
            return 0;
        }
    }
}