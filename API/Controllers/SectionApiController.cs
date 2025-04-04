using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionApiController : ControllerBase
    {
        private readonly ISectionInterface _section;
        public SectionApiController(ISectionInterface section)
        {
            _section = section;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var sections = await _section.GetAll();
            return Ok(sections);
        }

        [HttpGet]
        [Route("class_Read")]
        public async Task<IActionResult> class_Read()
        {
            var classes = await _section.GetAllClass();
            return Ok(classes);
        }

        [HttpGet]
        [Route("GetSectionById/{id}")]
        public async Task<IActionResult> GetSectionById(int id)
        {
            var section = await _section.GetOne(id);
            if (section == null)
                return BadRequest(new { success = false, message = "Section not found" });
            return Ok(section);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] t_section section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int status = await _section.Create(section);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Section created successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error creating section" });
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] t_section section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int status = await _section.Update(section);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Section updated successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error updating section" });
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int status = await _section.Delete(id);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Section deleted successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error deleting section" });
            }
        }
    }
}