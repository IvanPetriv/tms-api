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
    public class ChatMessageReactionsController : BaseController<ChatMessageReaction, ChatMessageReactionDTO, long> {

        private readonly string _tableName = nameof(ChatMessageReaction);

        public ChatMessageReactionsController(TmsMainContext dbContext, IMapper mapper, ILogger<ChatMessageReactionsController> logger) : base(dbContext, mapper, logger) { }


        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(long id) {
            return await base.GetById(id);
        }

        /// <summary>
        /// Retrieves all chat message reactions for the chat message by its ID
        /// </summary>
        /// <param name="id">ID of the chat message reaction</param>
        /// <returns>Objects with the specified chat message, if exist, otherwise 404 code</returns>
        [HttpGet("chat/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByChatId(long id) {
            var foundEntries = await dbContext.ChatMessageReactions
                .Where(e => e.MessageId == id)
                .ToListAsync();

            if (foundEntries.Count == 0) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(ChatMessageReaction), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(ChatMessageReaction), id)
                });
            }

            return Ok(mapper.Map<List<ChatMessageReactionDTO>>(foundEntries));
        }


        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] ChatMessageReactionDTO objectDTO) {
            return await base.Create(objectDTO);
        }

        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] ChatMessageReactionDTO objectDTO) {
            return await base.Update(objectDTO);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(long id) {
            return await base.Delete(id);
        }
    }
}
