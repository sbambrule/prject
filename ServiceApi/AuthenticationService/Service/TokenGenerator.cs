using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace AuthenticationService.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenGenerator : ITokenGenerator
    {
        IConfiguration configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public TokenGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string GeneratorJwtToken(string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Typ,userId),
                new Claim(JwtRegisteredClaimNames.Typ,userId),

                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Audience:key"]));
            var credit = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: configuration["Audience:lss"],
                audience: configuration["Audience:Aud"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: credit
                );

            var response = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            };
            return JsonConvert.SerializeObject(response);
        }
    }
}
