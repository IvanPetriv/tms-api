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
    public class TranslationVotesController(TmsMainContext dbContext,
                                        IMapper mapper,
                                        ILogger<ProjectsController> logger) : ControllerBase {
        // Controller properties
        private const string controllerName = nameof(TranslationVotesController);
        private const string tableName = nameof(TranslationVote);



        /// <summary>
        /// Retrieves the translation vote by its ID
        /// </summary>
        /// <param name="id">ID of the translation vote</param>
        /// <returns>Object with the specified ID, if exists, otherwise 404 code</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TranslationVoteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id) {
            var foundEntry = await dbContext.TranslationVotes.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, controllerName, tableName, id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(tableName, id)
                });
            }

            return Ok(mapper.Map<TranslationVoteDTO>(foundEntry));
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
                logger.LogWarning(LogMessage.NotFoundById, controllerName, nameof(SourceString), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(SourceString), id)
                });
            }

            return Ok(mapper.Map<List<TranslationVoteDTO>>(foundEntries));
        }



        /// <summary>
        /// Creates a translation vote with the given data
        /// </summary>
        /// <param name="objectDTO">Translation vote data</param>
        /// <returns>201 code, if successful; 409 code, if the ID already exists</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] TranslationVoteDTO objectDTO) {
            if (await dbContext.TranslationVotes.AnyAsync(u => u.Id == objectDTO.Id)) {
                logger.LogWarning(LogMessage.AlreadyExistsWithId, controllerName, tableName, objectDTO.Id);
                return Conflict(new {
                    Message = ResultMessage.AlreadyExistsWithId(tableName, objectDTO.Id)
                });
            }

            var entry = mapper.Map<TranslationVote>(objectDTO);
            await dbContext.TranslationVotes.AddAsync(entry);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, objectDTO);
        }



        /// <summary>
        /// Updates the translation vote with the given data
        /// </summary>
        /// <param name="objectDTO">Translation vote data</param>
        /// <returns>204 code if successful; 404 code, if doesn't exist</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] TranslationVoteDTO objectDTO) {
            var existingEntry = await dbContext.TranslationVotes.FindAsync(objectDTO.Id);
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
        /// Deletes the translation vote by its ID
        /// </summary>
        /// <param name="id">ID of the translation vote</param>
        /// <returns>204 code, if successful; 404 code, if doesn't exist</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id) {
            var foundEntry = await dbContext.TranslationVotes.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, controllerName, tableName, id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(tableName, id)
                });
            }

            dbContext.TranslationVotes.Remove(foundEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
