using APIMain.Authentication.UserDataObjects;
using APIMain.Configuration.Objects;
using BackendDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIMain.Controllers {
    [Route("api/login")]
    [ApiController]
    public class AuthenticationController(TmsMainContext dbContext,
                                          IConfiguration config,
                                          ILogger<AuthenticationController> logger) : ControllerBase {
        [HttpPost("token")]
        public IActionResult GetToken([FromBody] UserLoginData user, int expiresIn = 864000) {
            if (AuthenticateUser(user) is not User foundUser) {
                return NotFound("User was not found");
            }
            string token = GenerateToken(foundUser, expiresIn);
            return Ok(new {
                Token = token
            });
        }


        #region Helpers
        private User? AuthenticateUser(UserLoginData authData) {
            ArgumentNullException.ThrowIfNull(authData);

            // Checks if DB contains a user with the same username or email
            var binUserPassword = Encoding.UTF8.GetBytes(authData.Password);
            User? foundUser = dbContext.Users.FirstOrDefault(e => e.Username == authData.Login || e.Email == authData.Login);
            if (foundUser is null) {
                return null;
            }

            // Checks if passwords match
            var userLogin = dbContext.UserLogins.FirstOrDefault(e => e.UserId == foundUser.Id);
            if (userLogin is null) {
                return null;
            } else if (Encoding.UTF8.GetString(userLogin.Password).TrimEnd('\0') != Encoding.UTF8.GetString(binUserPassword).TrimEnd('\0')) {
                return null;
            }

            return foundUser;
        }

        private string GenerateToken(User user, int expiresIn) {
            // JWT config from appsettings.json and .env
            JwtSettings jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>()
                    ?? throw new NullReferenceException("appsettings.json does not have 'JwtSettings' property.");
            string jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
                ?? throw new NullReferenceException("Environment does not have JWT key");

            // Checks if configuration is correct
            if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32) {
                throw new ArgumentException("JWT key must be at least 32 characters long for security.");
            }

            // Prepares the data for the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Username),
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new("userid", user.Id.ToString())
            };

            // Creates the token
            var token = new JwtSecurityToken(jwtSettings.Issuer,
                                             jwtSettings.Audience,
                                             claims,
                                             expires: DateTime.Now.AddSeconds(expiresIn),
                                             signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }
}
