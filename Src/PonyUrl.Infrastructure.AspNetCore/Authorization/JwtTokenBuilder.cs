﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PonyUrl.Infrastructure.AspNetCore.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PonyUrl.Infrastructure.AspNetCore.Authorization
{
    public sealed class JwtTokenBuilder
    {
        private readonly IConfiguration _configuration;

        public JwtTokenBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<JwtTokenModel> GenerateJwtToken(string email, ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var refreshToken = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires.AddDays(AuthContstants.RefreshTokenDaysCount),
                signingCredentials: creds
            );

            var jwtToken = new JwtTokenModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireDateTime = expires,
                RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken),
                Type = AuthContstants.AuthenticationSchemes.Bearer
            };

            return await Task.FromResult(jwtToken);
        }
    }
}
