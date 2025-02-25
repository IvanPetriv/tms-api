using APIMain.Messages;
using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIMain.Controllers {
    [ApiController]
    [Authorize]
    [EnableCors("AllowFrontend")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ChatsController(TmsMainContext dbContext,
                                        IMapper mapper,
                                        ILogger<ChatsController> logger) : ControllerBase {
        // Controller properties
        private const string controllerName = nameof(ChatsController);
        private const string tableName = nameof(Chat);



        /// <summary>
        /// Retrieves the chat by its ID
        /// </summary>
        /// <param name="id">ID of the chat</param>
        /// <returns>Object with the specified ID, if exists, otherwise 404 code</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ChatDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id) {
            var foundEntry = await dbContext.Chats.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, controllerName, tableName, id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(tableName, id)
                });
            }

            return Ok(mapper.Map<ChatDTO>(foundEntry));
        }



        /// <summary>
        /// Retrieves all chats for the user by its ID
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <returns>Objects with the specified user, if exist, otherwise 404 code</returns>
        [HttpGet("user/{id}")]
        [ProducesResponseType(typeof(List<ChatDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(long id) {
            var foundEntries = await dbContext.Chats
                .Where(chat => chat.Users.Any(user => user.Id == id))
                .ToListAsync();

            if (foundEntries.Count == 0) {
                logger.LogWarning(LogMessage.NotFoundById, controllerName, nameof(BackendDB.Models.User), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(BackendDB.Models.User), id)
                });
            }

            return Ok(mapper.Map<List<ChatDTO>>(foundEntries));
        }



        /// <summary>
        /// Creates a chat with the given data
        /// </summary>
        /// <param name="objectDTO">Chat data</param>
        /// <returns>201 code, if successful; 409 code, if the ID already exists</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] ChatDTO objectDTO) {
            if (await dbContext.Chats.AnyAsync(u => u.Id == objectDTO.Id)) {
                logger.LogWarning(LogMessage.AlreadyExistsWithId, controllerName, tableName, objectDTO.Id);
                return Conflict(new {
                    Message = ResultMessage.AlreadyExistsWithId(tableName, objectDTO.Id)
                });
            }

            var entry = mapper.Map<Chat>(objectDTO);
            await dbContext.Chats.AddAsync(entry);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, objectDTO);
        }



        /// <summary>
        /// Updates the chat with the given data
        /// </summary>
        /// <param name="objectDTO">Chat data</param>
        /// <returns>204 code if successful; 404 code, if doesn't exist</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] ChatDTO objectDTO) {
            var existingEntry = await dbContext.Chats.FindAsync(objectDTO.Id);
            if (existingEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, controllerName, tableName, objectDTO.Id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(tableName, objectDTO.Id)
                });
            }

            mapper.Map(objectDTO, existingEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }



        /// <summary>
        /// Deletes the chat by its ID
        /// </summary>
        /// <param name="id">ID of the chat</param>
        /// <returns>204 code, if successful; 404 code, if doesn't exist</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id) {
            var foundEntry = await dbContext.Chats.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, controllerName, tableName, id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(tableName, id)
                });
            }

            dbContext.Chats.Remove(foundEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
