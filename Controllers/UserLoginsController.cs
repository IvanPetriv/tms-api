using APIMain.Messages;
using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIMain.Controllers {
    [ApiController]
    [Authorize]
    [Route("api/login")]
    [Produces("application/json")]
    public class UserLoginController(TmsMainContext dbContext,
                                     IMapper mapper,
                                     ILogger<UserLoginController> logger) : ControllerBase {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserLoginDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id) {
            var foundEntry = await dbContext.UserLogins.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(ControllerMessage.NotFoundById, this.GetType().Name, id);
                return NotFound(new {
                    Message = string.Format(ControllerMessage.NotFoundById, this.GetType().Name, id)
                });
            }
            return Ok(mapper.Map<UserLoginDTO>(foundEntry));
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] UserLoginDTO objectDTO) {
            if (await dbContext.UserLogins.AnyAsync(u => u.Id == objectDTO.Id)) {
                logger.LogWarning(ControllerMessage.AlreadyExistsWithId, this.GetType().Name, objectDTO.Id);
                return Conflict(new {
                    Message = string.Format(ControllerMessage.AlreadyExistsWithId, this.GetType().Name, objectDTO.Id)
                });
            }

            var entry = mapper.Map<UserLogin>(objectDTO);
            await dbContext.UserLogins.AddAsync(entry);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, objectDTO);
        }



        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] UserLoginDTO objectDTO) {
            var existingEntry = await dbContext.UserLogins.FindAsync(objectDTO.Id);
            if (existingEntry is null) {
                logger.LogWarning(ControllerMessage.NotFoundById, this.GetType().Name, objectDTO.Id);
                return NotFound(new {
                    Message = string.Format(ControllerMessage.NotFoundById, this.GetType().Name, objectDTO.Id)
                });
            }

            mapper.Map(objectDTO, existingEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id) {
            var foundEntry = await dbContext.UserLogins.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(ControllerMessage.NotFoundById, this.GetType().Name, id);
                return NotFound(new {
                    Message = string.Format(ControllerMessage.NotFoundById, this.GetType().Name, id)
                });
            }

            dbContext.UserLogins.Remove(foundEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
