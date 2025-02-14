using APIMain.Messages;
using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIMain.Authentication.Jwt;
using System.Diagnostics;

namespace APIMain.Controllers {
    [ApiController]
    [Authorize]
    [EnableCors("AllowFrontend")]
    [Route("api/project")]
    [Produces("application/json")]
    public class ProjectsController(TmsMainContext dbContext,
                                    IMapper mapper,
                                    ILogger<UserLoginController> logger) : ControllerBase {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id) {
            var foundEntry = await dbContext.Projects.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(ControllerMessage.NotFoundById,
                                    this.GetType().Name, nameof(Project), id);
                return NotFound(new {
                    Message = string.Format(ControllerMessage.NotFoundById,
                                            this.GetType().Name, nameof(Project), id)
                });
            }

            return Ok(mapper.Map<ProjectDTO>(foundEntry));
        }



        [HttpGet("user/{id}")]
        [ProducesResponseType(typeof(List<ProjectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllByUserId(int id) {
            // Checks if the user has a right to access this resource
            if (!this.VerifyTokenUser(id)) {
                logger.LogWarning(ControllerMessage.UnauthorizedAccess, this.GetType().Name);
                return Unauthorized(new {
                    Message = string.Format(ControllerMessage.UnauthorizedAccess, this.GetType().Name)
                });
            }

            // Checks if the user exists
            if (!await dbContext.Users.AnyAsync(e => e.Id == id)) {
                logger.LogWarning(ControllerMessage.NotFoundById,
                                    this.GetType().Name, nameof(BackendDB.Models.User), id);
                return NotFound(new {
                    Message = string.Format(ControllerMessage.NotFoundById,
                                            this.GetType().Name, nameof(BackendDB.Models.User), id)
                });
            }

            // Retrieves the data
            var foundProjects = await dbContext.Projects.Where(e => e.CreatedBy == id).ToListAsync();
            return Ok(mapper.Map<List<ProjectDTO>>(foundProjects));
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] ProjectDTO objectDTO) {
            if (await dbContext.Projects.AnyAsync(u => u.Id == objectDTO.Id)) {
                logger.LogWarning(ControllerMessage.AlreadyExistsWithId,
                                    this.GetType().Name, nameof(Project), objectDTO.Id);
                return Conflict(new {
                    Message = string.Format(ControllerMessage.AlreadyExistsWithId,
                                            this.GetType().Name, nameof(Project), objectDTO.Id)
                });
            }

            var entry = mapper.Map<Project>(objectDTO);
            await dbContext.Projects.AddAsync(entry);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = entry.Id }, objectDTO);
        }



        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromBody] ProjectDTO objectDTO) {
            var existingEntry = await dbContext.Projects.FindAsync(objectDTO.Id);
            if (existingEntry is null) {
                logger.LogWarning(ControllerMessage.NotFoundById,
                                    this.GetType().Name, nameof(Project), objectDTO.Id);
                return NotFound(new {
                    Message = string.Format(ControllerMessage.NotFoundById,
                                            this.GetType().Name, nameof(Project), objectDTO.Id)
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
            var foundEntry = await dbContext.Projects.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(ControllerMessage.NotFoundById,
                                    this.GetType().Name, nameof(Project), id);
                return NotFound(new {
                    Message = string.Format(ControllerMessage.NotFoundById,
                                            this.GetType().Name, nameof(Project), id)
                });
            }

            dbContext.Projects.Remove(foundEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
