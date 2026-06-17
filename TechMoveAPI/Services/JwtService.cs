using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/Hello-PROG7311/tree/main/10%20-%20JWT
Date: [n.d]
Date Accessed: 9 May 2026

Author: Punisher
URL: https://medium.com/@punisher70149/jwtservice-d834721cb0eb
Date: 3 July 2025
Date Accessed: 9 May 2026
*/
namespace TechMoveAPI.Services
{
    public class JwtService
    {
        private readonly IConfiguration _config;
        private readonly byte[] _key;

        // Constructor to initialize JWT service with configuration settings
        public JwtService(IConfiguration config)
        {
            _config = config;

            // Retrieve JWT secret key from configuration (supports Docker environment variables)
            var jwtKey = _config["TechMoveJwtKey"];

            // Ensure JWT key exists before generating tokens
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new Exception("JWT key is missing from configuration (TechMoveJwtKey)");

            _key = Encoding.UTF8.GetBytes(jwtKey);
        }

        // Method to generate a JWT token for a given user email
        public string GenerateToken(string email)
        {
            // Define user identity claims for the JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
            };

            // Create signing credentials using HMAC SHA256 security algorithm
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256
            );

            // Create JWT token with issuer, audience, expiration, and signing credentials
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            // Convert JWT token to string format for API response
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}