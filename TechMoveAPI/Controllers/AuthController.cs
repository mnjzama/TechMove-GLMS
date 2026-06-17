using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TechMoveAPI.Models;
using TechMoveAPI.ViewModels;
using TechMoveAPI.Services;

/*
Author: PROG7311-2026-EMWVL (Lecturer Repository)
URL: https://github.com/PROG7311-2026-EMWVL/MathAPI
Date: [n.d]
Date Accessed: 14 May 2026
*/
namespace TechMoveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly FirebaseAuthProvider _auth;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, JwtService jwtService)
        {
            _configuration = configuration;
            _jwtService = jwtService;

            // Read the Firebase API key from the application configuration
            var firebaseApiKey = _configuration["FirebaseMathApp"];

            // Initialize Firebase authentication provider
            _auth = new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey));
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                // Create a new Firebase user account
                var result = await _auth.CreateUserWithEmailAndPasswordAsync(
                    model.Email,
                    model.Password
                );

                // Generate a JWT token after successful registration
                var token = _jwtService.GenerateToken(result.User.Email);

                return Ok(new
                {
                    Email = result.User.Email,
                    Token = token
                });
            }
            catch (FirebaseAuthException ex)
            {
                // Deserialize Firebase error response for readable error messages
                var error = JsonSerializer.Deserialize<FirebaseErrorModel>(
                    ex.ResponseData
                );

                return BadRequest(error?.error?.message ?? "Registration failed");
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                // Authenticate the user with Firebase
                var result = await _auth.SignInWithEmailAndPasswordAsync(
                    model.Email,
                    model.Password
                );

                // Generate a JWT token after successful login
                var token = _jwtService.GenerateToken(result.User.Email);

                return Ok(new
                {
                    Email = result.User.Email,
                    Token = token
                });
            }
            catch (FirebaseAuthException ex)
            {
                // Deserialize Firebase error response for readable error messages
                var error = JsonSerializer.Deserialize<FirebaseErrorModel>(ex.ResponseData);

                return BadRequest(error?.error?.message ?? "Login failed");
            }
        }
    }
}