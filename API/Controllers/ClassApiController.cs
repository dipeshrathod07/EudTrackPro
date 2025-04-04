using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassApiController : ControllerBase
    {
        private readonly IClassInterface _class;
        public ClassApiController(IClassInterface classes)
        {
            _class = classes;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var classes = await _class.GetAll();
            return Ok(classes);
        }

        [HttpGet]
        [Route("GetClassById/{id}")]
        public async Task<IActionResult> GetClassById(int id)
        {
            var classData = await _class.GetOne(id);
            if (classData == null)
                return BadRequest(new { success = false, message = "Class not found" });
            return Ok(classData);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] t_class classData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int status = await _class.Create(classData);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Class created successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error creating class" });
            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] t_class classData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int status = await _class.Update(classData);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Class updated successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error updating class" });
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int status = await _class.Delete(id);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Class deleted successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error deleting class" });
            }
        }

    }
}