using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Edutrack;
using Edutrack.Models;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]

    public class MaterialApiController : ControllerBase
    {
        private readonly ITeachingMaterial _ITeachingMaterial;
        public MaterialApiController(ITeachingMaterial teachingMaterial)
        {
            _ITeachingMaterial = teachingMaterial;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadTeachingMaterial(IFormFile file, [FromForm] int teacherId, [FromForm] int subjectId)
        {
            System.Console.WriteLine("hello");
            Console.WriteLine("Received File: " + (file != null ? file.FileName : "No file"));
            System.Console.WriteLine(subjectId);
            System.Console.WriteLine(teacherId);
            if (file == null || file.Length == 0)
                return BadRequest(new { success = false, message = "No file uploaded" });

            try
            {
                string mvcProjectPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Mvc");
                string webRootPath = Path.Combine(mvcProjectPath, "wwwroot");
                string uploadsFolder = Path.Combine(webRootPath, "UploadedMaterials");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = $"{Guid.NewGuid()}_{file.FileName}";
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var material = new TeachingMaterial
                {
                    C_File_Name = fileName,
                    C_File_Type = file.ContentType,
                    C_Upload_Date = DateTime.UtcNow,
                    C_Subject_Id = subjectId,
                    C_Teacher_Id = teacherId,
                    C_File_Path = $"/UploadedMaterials/{fileName}"
                };

                var result = await _ITeachingMaterial.addMaterial(material);

                if (result.success)
                    return Ok(new { success = true, message = result.message });

                return BadRequest(new { success = false, message = result.message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal Server Error", error = ex.Message });
            }
        }


        [HttpGet("GetMaterialsByTeacher/{teacherId}")]
        public async Task<IActionResult> GetMaterialsByTeacher(int teacherId)
        {
            var result = await _ITeachingMaterial.GetMaterialsByTeacherId(teacherId);

            if (result.success)
                return Ok(new { success = true, message = result.message, data = result.Item3 });

            return NotFound(new { success = false, message = result.message });
        }

        [HttpGet("GetstudentByTeacher/{teacherId}")]
        public async Task<IActionResult> GetAllStudentByTeacher(int teacherId)
        {
            var (success, message, students) = await _ITeachingMaterial.Getstudentbyteacher(teacherId);

            if (success)
                return Ok(new { success, data = students });
            else
                return NotFound(new { success, message });
        }

        [HttpGet("GetTeacher/{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var (success, message, teacher) = await _ITeachingMaterial.GetTeacherById(id);

            if (success)
            {
                return Ok(new { success = true, message, teacher });
            }
            else
            {
                return NotFound(new { success = false, message });
            }
        }

        [HttpGet("GetNotification")]
        public async Task<IActionResult> GetNotification()
        {
            var notification = await _ITeachingMaterial.GetNotification();
            if (notification != null)
            {
                return Ok(new { success = true, notification });
            }
            else
            {
                return NotFound(new { success = false, message = "Notification not found" });
            }
        }
    }
}
