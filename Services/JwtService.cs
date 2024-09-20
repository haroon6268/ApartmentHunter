using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Apartments.Models;
using Microsoft.IdentityModel.Tokens;

namespace Apartments.Services{
    public class JwtService{
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration){
            _configuration = configuration;
        }
        public string GenerateToken(User user){
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.email)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddDays(90), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}