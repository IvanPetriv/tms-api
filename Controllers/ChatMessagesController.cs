using APIMain.Messages;
using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIMain.Authentication.Jwt;

namespace APIMain.Controllers {
    [ApiController]
    [Authorize]
    [EnableCors("AllowFrontend")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ChatMessagesController : BaseController<ChatMessage, ChatMessageDTO, long> {

        private readonly string _tableName = nameof(ChatMessage);

        public ChatMessagesController(TmsMainContext dbContext, IMapper mapper, ILogger<ChatMessagesController> logger) : base(dbContext, mapper, logger) { }


        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(long id) {
            return await base.GetById(id);
        }

        /// <summary>
        /// Retrieves all chat messages for the chat by its ID
        /// </summary>
        /// <param name="id">ID of the chat</param>
        /// <returns>Objects with the specified chat, if exist, otherwise 404 code</returns>
        [HttpGet("user/{id}")]
        [ProducesResponseType(typeof(List<ChatMessageDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByChatId(long id) {
            var foundEntries = await dbContext.ChatMessages
                .Where(chat => chat.Chat.Id == id)
                .ToListAsync();

            if (foundEntries.Count == 0) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(ChatMessage), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(ChatMessage), id)
                });
            }

            return Ok(mapper.Map<List<ChatMessageDTO>>(foundEntries));
        }


        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] ChatMessageDTO objectDTO) {
            return await base.Create(objectDTO);
        }

        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] ChatMessageDTO objectDTO) {
            return await base.Update(objectDTO);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(long id) {
            return await base.Delete(id);
        }
    }
}
