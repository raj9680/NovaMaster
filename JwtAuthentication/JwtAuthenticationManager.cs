using Imm.DAL.Data;
using Imm.DAL.Data.Table;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Auth
{
    public class JwtAuthenticationManager
    {
        private readonly JwtSettings _settings;
        public JwtAuthenticationManager(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }
        public IDictionary<string, string> Authenticate(AspNetUsers user, AspNetRoles role)
        {
            IDictionary<string, string> tokenn = new Dictionary<string, string>();
            var tokenhandler = new JwtSecurityTokenHandler();

            var tokenkey = Encoding.UTF8.GetBytes(_settings.securitykey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] { 
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, role.Role),
                        new Claim(ClaimTypes.Name, user.UserId.ToString())
                    }),
                Expires = DateTime.Now.AddMinutes(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(token);
            tokenn.Add("token", finaltoken);
            tokenn.Add("role", role.Role);
            return tokenn;
        }
    }
}