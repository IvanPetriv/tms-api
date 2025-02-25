using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace APIMain.Controllers {
    [ApiController]
    [Authorize]
    [EnableCors("AllowFrontend")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LocalesController : BaseController<Locale, LocaleDTO, short> {

        private readonly string _tableName = nameof(Locale);

        public LocalesController(TmsMainContext dbContext, IMapper mapper, ILogger<LocalesController> logger) : base(dbContext, mapper, logger) { }



        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(short id) {
            return await base.GetById(id);
        }
    }
}
