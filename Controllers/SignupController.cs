using APIMain.Authentication.UserDataObjects;
using AutoMapper;
using BackendDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIMain.Controllers {
    [Route("api/register")]
    [ApiController]
    public class SignupController(TmsMainContext dbContext,
                                    IConfiguration config,
                                    IMapper mapper,
                                    ILogger<AuthenticationController> logger) : ControllerBase {
        [HttpPost("manual")]
        public async Task<IActionResult> RegisterManually([FromBody] UserSignUpData userData) {
            if (await dbContext.Users.AnyAsync(e => e.Username == userData.Username)) {
                return Conflict(new {
                    Message = "User with this username already exists"
                });
            }

            var mappedUser = mapper.Map<User>(userData);
            await dbContext.Users.AddAsync(mappedUser);
            await dbContext.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("oauth")]
        public async Task<IActionResult> RegisterWithOAuth([FromBody] UserSignUpData userData) {
            return Ok();
        }
    }
}
