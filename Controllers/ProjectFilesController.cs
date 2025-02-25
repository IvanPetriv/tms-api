using AutoMapper;
using BackendDB.ModelDTOs;
using BackendDB.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIMain.Controllers {
    public class ProjectFilesController : BaseController<ProjectFile, ProjectFileDTO, long> {

        private readonly string _tableName = nameof(ProjectFile);

        public ProjectFilesController(TmsMainContext dbContext, IMapper mapper, ILogger<ProjectFilesController> logger) : base(dbContext, mapper, logger) { }



        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(long id) {
            return await base.GetById(id);
        }

        [HttpPost]
        public override async Task<IActionResult> Create([FromBody] ProjectFileDTO objectDTO) {
            return await base.Create(objectDTO);
        }

        [HttpPut]
        public override async Task<IActionResult> Update([FromBody] ProjectFileDTO objectDTO) {
            return await base.Update(objectDTO);
        }

        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete(long id) {
            return await base.Delete(id);
        }
    }
}
