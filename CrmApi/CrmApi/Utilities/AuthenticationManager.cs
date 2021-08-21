using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CrmApi.Contracts;
using CrmApi.DataTransferObjects;
using CrmApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CrmApi.Utilities
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;

        public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> ValidateUser(UserDto userAuthDto)
        {
            _user = await _userManager.FindByEmailAsync(userAuthDto.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, userAuthDto.Password));
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = CreateSigningCredentials();
            var claims = await GetAuthClaims();
            var token = GenerateToken(signingCredentials, claims);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SigningCredentials CreateSigningCredentials()
        {
            var secret = AuthenticationUtilities.GetSymmetricSecurityKey(_configuration["Jwt:SecretKey"]);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetAuthClaims()
        {
            var claims = new List<Claim>() 
            {    
                new Claim(ClaimTypes.Name, _user.UserName),
            };

            var roles = await _userManager.GetRolesAsync(_user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        private JwtSecurityToken GenerateToken(SigningCredentials credentials, List<Claim> claims)
        {
            return new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:LifeTime"])),
                claims: claims,
                signingCredentials: credentials
            );
        }
    }
}