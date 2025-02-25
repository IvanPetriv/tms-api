using APIMain.Messages;
using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIMain.Controllers {
    public class TranslationsController : BaseController<Translation, TranslationDTO, long> {

        private readonly string _tableName = nameof(Translation);

        public TranslationsController(TmsMainContext dbContext, IMapper mapper, ILogger<TranslationsController> logger) : base(dbContext, mapper, logger) { }



        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(long id) {
            return await base.GetById(id);
        }

        /// <summary>
        /// Retrieves all translations for the source string by its ID
        /// </summary>
        /// <param name="id">ID of the source string</param>
        /// <returns>Objects with the specified source string, if exist, otherwise 404 code</returns>
        [HttpGet("source/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySourceStringId(long id) {
            var foundEntries = await dbContext.Translations
                .Where(e => e.SourceStringId == id)
                .ToListAsync();

            if (foundEntries.Count == 0) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(SourceString), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(SourceString), id)
                });
            }

            return Ok(mapper.Map<List<TranslationDTO>>(foundEntries));
        }

        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] TranslationDTO objectDTO) {
            return await base.Create(objectDTO);
        }

        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] TranslationDTO objectDTO) {
            return await base.Update(objectDTO);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(long id) {
            return await base.Delete(id);
        }
    }
}
