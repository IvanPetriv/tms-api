using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using APIMain.Messages;
using Microsoft.Extensions.Logging;

namespace APIMain.Controllers {
    public class ChatsTestController(TmsMainContext dbContext,
                                 IMapper mapper,
                                 ILogger<ChatsTestController> logger)
        : BaseController<Chat, ChatDTO, int>(dbContext, mapper, logger) {

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(int id) {
            return await base.GetById(id);
        }



        /// <summary>
        /// Retrieves all chats for the user by its ID
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <returns>Objects with the specified user, if exist, otherwise 404 code</returns>
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetByUserId(long id) {
            var foundEntries = await dbContext.Chats
                .Where(chat => chat.Users.Any(user => user.Id == id))
                .AsNoTracking()
                .ToListAsync();

            if (foundEntries.Count == 0) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(BackendDB.Models.User), id);
                return NotFound(new { Message = ResultMessage.NotFoundById(nameof(BackendDB.Models.User), id) });
            }

            return Ok(mapper.Map<List<ChatDTO>>(foundEntries));
        }



        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] ChatDTO objectDTO) {
            return await base.Create(objectDTO);
        }



        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] ChatDTO objectDTO) {
            return await base.Update(objectDTO);
        }



        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id) {
            return await base.Delete(id);
        }
    }
}
