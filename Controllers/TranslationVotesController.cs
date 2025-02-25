using APIMain.Messages;
using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIMain.Controllers {
    public class TranslationVotesController : BaseController<TranslationVote, TranslationVoteDTO, long> {

        private readonly string _tableName = nameof(TranslationVote);

        public TranslationVotesController(TmsMainContext dbContext, IMapper mapper, ILogger<TranslationVotesController> logger) : base(dbContext, mapper, logger) { }



        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(long id) {
            return await base.GetById(id);
        }

        /// <summary>
        /// Retrieves all translation votess for the translation by its ID
        /// </summary>
        /// <param name="id">ID of the translation</param>
        /// <returns>Objects with the specified translation, if exist, otherwise 404 code</returns>
        [HttpGet("translation/{id}")]
        [ProducesResponseType(typeof(List<TranslationDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySourceStringId(long id) {
            var foundEntries = await dbContext.TranslationVotes
                .Where(e => e.TranslationId == id)
                .ToListAsync();

            if (foundEntries.Count == 0) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(SourceString), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(SourceString), id)
                });
            }

            return Ok(mapper.Map<List<TranslationVoteDTO>>(foundEntries));
        }

        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] TranslationVoteDTO objectDTO) {
            return await base.Create(objectDTO);
        }

        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] TranslationVoteDTO objectDTO) {
            return await base.Update(objectDTO);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(long id) {
            return await base.Delete(id);
        }
    }
}
