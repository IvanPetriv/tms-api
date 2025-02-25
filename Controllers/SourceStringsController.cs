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
    public class SourceStringsController(TmsMainContext dbContext,
                                        IMapper mapper,
                                        ILogger<ProjectsController> logger) : ControllerBase {
        // Controller properties
        private const string controllerName = nameof(SourceStringsController);
        private const string tableName = nameof(SourceString);



        /// <summary>
        /// Retrieves the source string by its ID
        /// </summary>
        /// <param name="id">ID of the source string</param>
        /// <returns>Object with the specified ID, if exists, otherwise 404 code</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SourceStringDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id) {
            var foundEntry = await dbContext.SourceStrings.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, controllerName, tableName, id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(tableName, id)
                });
            }

            return Ok(mapper.Map<SourceStringDTO>(foundEntry));
        }



        /// <summary>
        /// Creates a source string with the given data
        /// </summary>
        /// <param name="objectDTO">Source string data</param>
        /// <returns>201 code, if successful; 409 code, if the ID already exists</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] SourceStringDTO objectDTO) {
            if (await dbContext.SourceStrings.AnyAsync(u => u.Id == objectDTO.Id)) {
                logger.LogWarning(LogMessage.AlreadyExistsWithId, controllerName, tableName, objectDTO.Id);
                return Conflict(new {
                    Message = ResultMessage.AlreadyExistsWithId(tableName, objectDTO.Id)
                });
            }

            var entry = mapper.Map<SourceString>(objectDTO);
            await dbContext.SourceStrings.AddAsync(entry);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, objectDTO);
        }



        /// <summary>
        /// Updates the source string with the given data
        /// </summary>
        /// <param name="objectDTO">Source string data</param>
        /// <returns>204 code if successful; 404 code, if doesn't exist</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] SourceStringDTO objectDTO) {
            var existingEntry = await dbContext.SourceStrings.FindAsync(objectDTO.Id);
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
        /// Deletes the source string by its ID
        /// </summary>
        /// <param name="id">ID of the source string</param>
        /// <returns>204 code, if successful; 404 code, if doesn't exist</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id) {
            var foundEntry = await dbContext.SourceStrings.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, controllerName, tableName, id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(tableName, id)
                });
            }

            dbContext.SourceStrings.Remove(foundEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
