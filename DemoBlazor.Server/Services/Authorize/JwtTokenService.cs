using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoBlazor.Server.Services.Authorize
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;
        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// Build the token used for authentication
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string BuildToken(string email)
        {
            // Create a claim based on the users email. You can add more claims like id's and other infor
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // creates a key from our private key that will be used in the security algorithm next
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // Credentials that are encrypted which can only be created by our server using the private key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // this is the actual token that will be issued to the user
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:ExpireTime"])),
                signingCredentials: creds );

            // return the token in the correct format using JwtSecurityHandler
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
