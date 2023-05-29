﻿using AuthDemo2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthDemo2.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<IdentityUser> userManager,IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<bool> Register(LoggedUser user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.UserName
            };

            var result = await _userManager.CreateAsync(identityUser,user.Password);
            return result.Succeeded;
        }

        public async Task<bool> Login(LoggedUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.UserName);
            if (identityUser == null)
            {
                return false;
            }
            return  await _userManager.CheckPasswordAsync(identityUser,user.Password);
        }

        public Task<string> GenerateToken(LoggedUser user)
        {
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.UserName),
                new Claim(ClaimTypes.Role,"Admin")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken
                (
                    issuer: _config.GetSection("Jwt:Issuer").Value,
                    audience: _config.GetSection("Jwt:Audience").Value,
                    expires: DateTime.Now.AddMinutes(60),
                    claims: claims,
                    signingCredentials: signingCreds
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return Task.FromResult(tokenString);
        }
    }
}
