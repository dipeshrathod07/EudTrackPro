using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Models;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentApiController : ControllerBase
    {
        private readonly IStudentInterface _student;
        public StudentApiController(IStudentInterface student)
        {
            _student = student;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {

            List<t_student> list;
            list = await _student.GetAll();

            return Ok(list);
        }

        [HttpGet]
        [Route("StudentList")]
        public async Task<IActionResult> StudentList()
        {
            var data = await _student.GetData();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentById(string id)
        {
            var student = await _student.GetOne(id);
            if (student == null)
                return BadRequest(new { success = false, message = "There was no contact found" });
            return Ok(student);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] t_student student)
        {
            if (student.StudentImage != null && student.StudentImage.Length > 0)
            {

                var fileName = student.c_email + Path.GetExtension(student.StudentImage.FileName);

                var filePath = Path.Combine("../MVC/wwwroot/student_images", fileName);
                student.c_image = fileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    student.StudentImage.CopyTo(stream);
                }
            }
            var status = await _student.Register(student);


            if (status == 1)
            {
                // return Ok(new { success = true, message = "User Registered" });
                try
                {
                    string toEmail = student.c_email;
                    string subject = "Your Account Password";
                    string body = $"Dear {student.c_full_name},\n\nYour account has been successfully registered.\n\nYour password is: {student.c_password}\n\nPlease keep it secure.";

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("jeminlad2003@gmail.com", "pkerjfvwgxwsmezw"); // Use App Password here
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("jeminlad2003@gmail.com");
                            mail.To.Add(toEmail);
                            mail.Subject = subject;
                            mail.Body = body;

                            smtp.Send(mail);
                        }
                    }

                    return Ok(new { success = true, message = "User Registered. Password sent to email." });
                }
                catch (Exception ex)
                {
                    return Ok(new { success = true, message = "User Registered, but failed to send password email. Error: " + ex.Message });
                }

            }
            else if (status == 0)
            {

                return Ok(new { success = false, message = "User Already Exist" });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = "There was some error while Registration"
                });

            }
        }

        [HttpPut]
        [Route("Update/{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromForm] t_student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            if (student.StudentImage != null && student.StudentImage.Length > 0)
            {
                var fileName = student.c_email + Path.GetExtension(student.StudentImage.FileName);
                var filePath = Path.Combine("../MVC/wwwroot/student_images", fileName);
                student.c_image = fileName;
                System.IO.File.Delete(filePath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    student.StudentImage.CopyTo(stream);
                }
            }

            Console.WriteLine(student.c_password);
            Console.WriteLine(id);

            var st = await _student.GetOne((id).ToString());
            Console.WriteLine(st.c_password);

            if (st.c_password != student.c_password)
            {
                int status = await _student.Update(student);
                if (status == 1)
                {
                    // return Ok(new { success = true, message = "Contact Updated Successfully" });
                    try
                    {
                        string toEmail = student.c_email;
                        string subject = "Your Account Password";
                        string body = $"Dear {student.c_full_name},\n\nYour account has been successfully registered.\n\nYour password is: {student.c_password}\n\nPlease keep it secure.";

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential("jeminlad2003@gmail.com", "pkerjfvwgxwsmezw"); // Use App Password here
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;

                            using (MailMessage mail = new MailMessage())
                            {
                                mail.From = new MailAddress("jeminlad2003@gmail.com");
                                mail.To.Add(toEmail);
                                mail.Subject = subject;
                                mail.Body = body;

                                smtp.Send(mail);
                            }
                        }

                        return Ok(new { success = true, message = "User Registered. Password sent to email." });
                    }
                    catch (Exception ex)
                    {
                        return Ok(new { success = true, message = "User Registered, but failed to send password email. Error: " + ex.Message });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "There was some error while Update Contact" });
                }
            }
            else
            {
                int status = await _student.Update(student);
                if (status == 1)
                {
                    return Ok(new { success = true, message = "Contact Updated Successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "There was some error while Update Contact" });
                }
            }
        }

        [HttpGet]
        [Route("teacher_Read")]
        public async Task<IActionResult> teacher_Read()
        {
            var teacher = await _student.GetAllTeacher();
            return Ok(teacher);
        }

        [HttpGet]
        [Route("class_Read")]
        public async Task<IActionResult> class_Read()
        {
            var classes = await _student.GetAllClass();
            return Ok(classes);
        }

        [HttpGet]
        [Route("section_Read")]
        public async Task<IActionResult> section_Read(string id)
        {
            var section = await _student.GetSectionByClass(id);
            return Ok(section);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            int status = await _student.Delete(id);
            if (status == 1)
            {
                return Ok(new { success = true, message = "Student Deleted Successfully!!!!!" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There was some error while deleting the student" });
            }
        }

        [HttpGet]
        [Route("ExportStudentsToPDF")]
        public async Task<IActionResult> ExportStudentsToPDF()
        {
            try
            {
                var fileBytes = await _student.ExportStudentsToPDF();
                return File(fileBytes, "application/pdf", "Student_List.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExportStudentsToPDF: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while exporting students to PDF." });
            }
        }

        #region STUDENT

        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _student.GetStudent(id);
            if (student != null)
            {
                return Ok(new { success = true, student });
            }
            else
            {
                return BadRequest(new { success = false, message = "Student not found" });
            }
        }


        [HttpGet("GetByStudent/{studentId}")]
        public async Task<IActionResult> GetSyllabusTrackingByStudent(int studentId)
        {
            try
            {
                var result = await _student.GetSyllabusTrackingByStudent(studentId);

                // Prevents null reference exception and handles empty result set
                if (result == null)
                {
                    return NotFound(new { message = "No syllabus tracking records found for this student." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the data.", error = ex.Message });
            }
        }
        [HttpPost]
        [Route("AddFeedback")]
        public async Task<IActionResult> AddFeedback([FromBody] t_teacherFeedback feedback)
        {
            Console.WriteLine("hello ");
            // if (!ModelState.IsValid)
            // {                
            //     return BadRequest(ModelState);
            // }           
            var status = await _student.teacherFeedback(feedback);

            if (status == 1)
            {
                return Ok(new { success = true, message = "Feedback Insterted Successfully!!!!!" });
            }
            else
            {
                return BadRequest(new { success = false, message = "There was some error while adding the Feedback" });
            }
        }
        [HttpGet]
        [Route("SList")]
        public async Task<ActionResult> SList()
        {
            List<vm_subjectimetable> contacts = await
            _student.GetTaskByClass((6).ToString());
            return Ok(contacts);
        }

        [HttpGet("GetMaterialsByTeacher/{teacherId}")]
        public async Task<IActionResult> GetMaterialsByTeacher(int teacherId)
        {
            var result = await _student.GetMaterialsByTeacherId(teacherId);

            if (result.success)
                return Ok(new { success = true, message = result.message, data = result.Item3 });

            return NotFound(new { success = false, message = result.message });
        }

        [HttpGet]
        [Route("GetStudentIdByUserId")]
        public async Task<IActionResult> GetStudentIdByUserId()
        {
            int userID = (int)HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine(userID + "uuseid hai");
            int studentId = await _student.GetStudentIdByUserId(userID);
            if (studentId > 0)
            {
                System.Console.WriteLine(studentId + "student hai");

                return Ok(new { success = true, studentId });
            }
            else
            {
                return BadRequest(new { success = false, message = "Student not found for this UserId" });
            }
        }

        #endregion
    }
}
