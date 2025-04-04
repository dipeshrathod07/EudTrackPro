using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamApiController : ControllerBase
    {
        private readonly IExamInterface _exam;
        public ExamApiController(IExamInterface exam)
        {
            _exam = exam;
        }


        [HttpPost]
        [Route("AddExam")]
        public async Task<IActionResult> AddExam([FromForm] t_exam exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (exam.ExamFile != null && exam.ExamFile.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf", ".docx", ".doc" };
                var fileExtension = Path.GetExtension(exam.ExamFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { success = false, message = "Only PDF or Word files are allowed." });
                }

                var fileName = Path.GetFileName(exam.ExamFile.FileName); // Store actual file name
                var filePath = Path.Combine("../MVC/wwwroot/exam_files", fileName);

                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Delete existing file if exists
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await exam.ExamFile.CopyToAsync(stream);
                }

                exam.c_exam_image = fileName; // Save the actual file name in DB
            }

            var status = await _exam.AddExam(exam);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Exam file uploaded successfully!" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error while uploading the exam file." });
            }
        }

    }
}
