using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace MyApp.Namespace
{

    [Route("api/[controller]")]
    [ApiController]
    public class TeacherApiController : ControllerBase
    {
        private readonly ITeacherInterface _teacher;
        public TeacherApiController(ITeacherInterface teacher)
        {
            _teacher = teacher;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var contact = await _teacher.GetOne(id);
            if (contact == null)
                return BadRequest(new { success = false, message = "There was no teacher found" });
            return Ok(contact);
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _teacher.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public class UpdateStatusModel
        {
            public string c_status { get; set; }
        }


        [HttpPost]
        [Route("UpdateStatus/{id}")]  // Route expects id in the URL
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusModel statusModel)
        {
            try
            {
                if (statusModel == null || string.IsNullOrEmpty(statusModel.c_status))
                {
                    return BadRequest("Invalid request data");
                }

                // Debugging: Check if ID is received correctly
                Console.WriteLine($"Received ID: {id}, Status: {statusModel.c_status}");

                int result = await _teacher.UpdateStatus(id.ToString(), statusModel.c_status);
                if (result == 1)
                {
                    return Ok(new { message = "Status updated successfully" });
                }
                else
                {
                    return NotFound("Teacher not found or update failed");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(string id)
        {
            int status = await _teacher.Delete(id);
            if (status == 1)
            {
                return Ok(new { success = true, message = " Deleted Successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There Is Some Error while Delete " });
            }
        }

        [HttpGet]
        [Route("GetPendingTeacherCount")]
        public async Task<int> GetPendingTeacherCount()
        {
            return await _teacher.GetPendingTeacherCount();
        }

        [HttpGet]
        [Route("ExportTeachersToPDF")]
        public async Task<IActionResult> ExportTeachersToPDF()
        {
            try
            {
                var fileBytes = await _teacher.ExportTeachersToPDF();
                return File(fileBytes, "application/pdf", "Teacher_List.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExportTeachersToPDF: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while exporting teachers to PDF." });
            }
        }
    }
}
