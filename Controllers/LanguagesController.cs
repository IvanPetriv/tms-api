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
    public class LanguagesController(TmsMainContext dbContext,
                                    IMapper mapper,
                                    ILogger<ProjectsController> logger) : ControllerBase {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LanguageDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(short id) {
            var foundEntry = await dbContext.Languages.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(Language), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(Language), id)
                });
            }

            return Ok(mapper.Map<ProjectDTO>(foundEntry));
        }
    }
}
