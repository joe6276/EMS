using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EMS.Models;
using EMS.Services.Interfaces;
using EMS.Utility;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EMS.Services
{
    public class JwtService : IJwt
    {
        private readonly JwtOptions _jwtoptions;
        public JwtService(IOptions<JwtOptions> options)
        {
            
            _jwtoptions = options.Value;
        }

        public string GenerateToken(User user, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtoptions.SecretKey));
            //Signing Credentials
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //payload-data
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add( new Claim(ClaimTypes.Role, role));


            // Token Descriptor

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _jwtoptions.Issuer,
                Audience = _jwtoptions.Audience,
                Expires = DateTime.UtcNow.AddHours(3),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = cred
            };


            var tokenHandler = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(tokenHandler);
        }
    }
}
