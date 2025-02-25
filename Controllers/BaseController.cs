using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using APIMain.Messages;
using BackendDB.Models;

namespace APIMain.Controllers {
    [ApiController]
    [Authorize]
    [EnableCors("AllowFrontend")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public abstract class BaseController<TEntity, TDto, TId> : ControllerBase
                    where TEntity : class where TDto : class where TId : struct {

        protected readonly TmsMainContext dbContext;
        protected readonly IMapper mapper;
        protected readonly ILogger<BaseController<TEntity, TDto, TId>> logger;
        private readonly string _tableName = typeof(TEntity).Name;

        public BaseController(TmsMainContext dbContext, IMapper mapper, ILogger logger) : base() {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = (ILogger<BaseController<TEntity, TDto, TId>>)logger;
        }



        /// <summary>
        /// Base method for getting an object by its ID
        /// </summary>
        /// <typeparam name="T">short, int or long</typeparam>
        /// <param name="id">ID of the object</param>
        /// <returns>Object</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> GetById(TId id) {
            var foundEntry = await dbContext.Set<TEntity>().FindAsync(id);

            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, _tableName, id);
                return NotFound(new { Message = ResultMessage.NotFoundById(_tableName, Convert.ToInt64(id)) });
            }

            return Ok(mapper.Map<TDto>(foundEntry));
        }



        /// <summary>
        /// Base method for creating an object
        /// </summary>
        /// <param name="objectDTO"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public virtual async Task<IActionResult> Create([FromBody] TDto objectDTO) {
            var entry = mapper.Map<TEntity>(objectDTO);
            await dbContext.Set<TEntity>().AddAsync(entry);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = ((dynamic)entry).Id }, objectDTO);
        }



        /// <summary>
        /// Base method for updating the object
        /// </summary>
        /// <param name="objectDTO"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Update([FromBody] TDto objectDTO) {
            var id = ((dynamic)objectDTO).Id;
            var existingEntry = await dbContext.Set<TEntity>().FindAsync(id);

            if (existingEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, _tableName, 3);
                return NotFound(new { Message = ResultMessage.NotFoundById(_tableName, id) });
            }

            mapper.Map(objectDTO, existingEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }



        /// <summary>
        /// Base method for deleting the object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual async Task<IActionResult> Delete(TId id) {
            var foundEntry = await dbContext.Set<TEntity>().FindAsync(id);

            if (foundEntry is null) {
                logger.LogWarning(LogMessage.NotFoundById, this.GetType().Name, _tableName, id);
                return NotFound(new { Message = ResultMessage.NotFoundById(_tableName, Convert.ToInt64(id)) });
            }

            dbContext.Set<TEntity>().Remove(foundEntry);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
