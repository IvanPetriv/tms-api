using APIMain.Messages;
using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIMain.Authentication.Jwt;

namespace APIMain.Controllers {
    public class ProjectsController : BaseController<Project, ProjectDTO, int> {

        private readonly string _tableName = nameof(Project);

        public ProjectsController(TmsMainContext dbContext, IMapper mapper, ILogger<ProjectsController> logger) : base(dbContext, mapper, logger) { }



        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(int id) {
            return await base.GetById(id);
        }

        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllByUserId(int id) {
            // Checks if the user has a right to access this resource
            if (!this.VerifyTokenUser(id)) {
                logger.LogWarning(LogMessage.UnauthorizedAccess, this.GetType().Name);
                return Unauthorized(new {
                    Message = string.Format(ResultMessage.UnauthorizedAccess())
                });
            }

            // Checks if the user exists
            if (!await dbContext.Users.AnyAsync(e => e.Id == id)) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(BackendDB.Models.User), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(BackendDB.Models.User), id)
                });
            }

            // Retrieves the data
            var foundProjects = await dbContext.Projects.Where(e => e.CreatedBy == id).ToListAsync();
            return Ok(mapper.Map<List<ProjectDTO>>(foundProjects));
        }

        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] ProjectDTO objectDTO) {
            return await base.Create(objectDTO);
        }

        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] ProjectDTO objectDTO) {
            return await base.Update(objectDTO);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id) {
            return await base.Delete(id);
        }



        /*[HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id) {
            var foundEntry = await dbContext.Projects.FindAsync(id);
            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(Project), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(Project), id)
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
                logger.LogWarning(LogMessage.UnauthorizedAccess, this.GetType().Name);
                return Unauthorized(new {
                    Message = string.Format(ResultMessage.UnauthorizedAccess())
                });
            }

            // Checks if the user exists
            if (!await dbContext.Users.AnyAsync(e => e.Id == id)) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(BackendDB.Models.User), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(BackendDB.Models.User), id)
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
                logger.LogWarning(LogMessage.AlreadyExistsWithId, this.GetType().Name, nameof(Project), objectDTO.Id);
                return Conflict(new {
                    Message = ResultMessage.AlreadyExistsWithId(nameof(Project), objectDTO.Id)
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
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(Project), objectDTO.Id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(Project), objectDTO.Id)
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
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(Project), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(Project), id)
                });
            }

            dbContext.Projects.Remove(foundEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }*/
    }
}
