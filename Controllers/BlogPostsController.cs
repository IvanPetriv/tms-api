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
    public class BlogPostsController : BaseController<BlogPost, BlogPostDTO, int> {

        private readonly string _tableName = nameof(BlogPost);

        public BlogPostsController(TmsMainContext dbContext, IMapper mapper, ILogger<BlogPostsController> logger) : base(dbContext, mapper, logger) { }


        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(int id) {
            return await base.GetById(id);
        }

        /// <summary>
        /// Retrieves all blog posts for the user by its ID
        /// </summary>
        /// <param name="id">ID of the user</param>
        /// <returns>Objects with the specified user, if exist, otherwise 404 code</returns>
        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUserId(int id) {
            var foundEntries = await dbContext.BlogPosts
                .Where(e => e.UserId == id)
                .ToListAsync();

            if (foundEntries.Count == 0) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, nameof(BlogPost), id);
                return NotFound(new {
                    Message = ResultMessage.NotFoundById(nameof(BlogPost), id)
                });
            }

            return Ok(mapper.Map<List<BlogPostDTO>>(foundEntries));
        }


        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] BlogPostDTO objectDTO) {
            return await base.Create(objectDTO);
        }

        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] BlogPostDTO objectDTO) {
            return await base.Update(objectDTO);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(int id) {
            return await base.Delete(id);
        }
    }
}
