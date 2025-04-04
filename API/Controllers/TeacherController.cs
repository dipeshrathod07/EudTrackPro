using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Edutrack;
using Edutrack.Models;
using System.Threading.Tasks;
using System.Text.Json;


namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherInterface _teacherService;

        public TeacherController(ITeacherInterface teacherService)
        {
            _teacherService = teacherService;
        }



        #region Subject Schedular
        [HttpPost("AddEvent")]
        public async Task<IActionResult> AddSchedulings([FromBody] t_Schedular schedulingData)
        {
            if (schedulingData == null)
            {
                return BadRequest("Invalid data received.");
            }
            int result = await _teacherService.AddScheduling(schedulingData);
            if (result == 1)
            {
                return Ok("Data Submitted Successfully");
            }
            return BadRequest("From AddEvent Controller : There was some Error while inserting the event");
        }

        [HttpGet("GetSchedulings/{teacherId}")]
        public async Task<IActionResult> GetSchedulings([FromQuery] int teacherId, [FromQuery] int classId, [FromQuery] int sectionId)
        {
            List<t_Schedular> schedulings = new List<t_Schedular>();
            schedulings = await _teacherService.GetSchedulings(teacherId, classId, sectionId);
            if (schedulings == null)
            {
                return BadRequest("The data is not fetched");
            }
            return Ok(schedulings);
        }

        [HttpPut("UpdateSchedulings/{id}")]
        public async Task<IActionResult> UpdateSchedulings(int id, [FromBody] t_Schedular scheduleData)
        {

            t_Schedular schedulings = scheduleData;
            schedulings.C_TimeTable_Id = id;
            int result = await _teacherService.UpdateScheduling(schedulings);
            if (result == 0)
            {
                return BadRequest("There is some error while Updating the data");
            }
            return Ok("Data Updated Successfully");
        }

        [HttpDelete("DeleteSchedulings/{id}")]
        public async Task<IActionResult> UpdateSchedulings(int id)
        {

            int result = await _teacherService.DeleteSchedule(id);
            if (result == 0)
            {
                return BadRequest("There is some error while Deleting the data");
            }
            return Ok("Data Deleted Successfully");
        }

        [HttpGet("GetClasses")]
        public async Task<IActionResult> GetClasses()
        {
            List<t_Class> Classes = new List<t_Class>();
            Classes = await _teacherService.GetClasses();
            if (Classes.Count != null)
            {
                return Ok(Classes);
            }
            else
            {
                return BadRequest("Class Data Not Fetched");
            }
        }

        [HttpGet("GetSectionsByClass/{id}")]
        public async Task<IActionResult> GetSectionsByClass(int id)
        {
            List<t_Section> Sections = new List<t_Section>();
            Sections = await _teacherService.GetSections(id);
            if (Sections.Count != null)
            {
                return Ok(Sections);
            }
            else
            {
                return BadRequest("Sections Data Not Fetched");
            }
        }

        #endregion

        #region teacher profile
        [HttpGet("GetTeacher")]
        public async Task<IActionResult> GetTeacher()
        {
            var userID = HttpContext.Session.GetInt32("UserId");
            if (userID == null)
            {
                return BadRequest(new { success = false, message = "User not logged in." });
            }

            var result = await _teacherService.GetTeacher(userID.Value);


            if (result != null)
            {
                return Ok(new { success = true, message = "Teacher found", data = result });
            }
            else
            {
                return BadRequest(new { success = false, message = "Teacher not found" });
            }
        }

        [HttpPut("UpdateTeacher")]
        public async Task<IActionResult> UpdateTeacher([FromBody] t_UpdateTeacher teacher)
        {

            var userID = HttpContext.Session.GetInt32("UserId");


            if (userID == null)
            {
                return BadRequest(new { success = false, message = "User not logged in." });
            }


            var result = await _teacherService.UpdateTeacher(teacher, userID.Value);

            if (result == 1)
            {
                return Ok(new { success = true, message = "Updated Successfully" });
            }
            else
            {
                return BadRequest(new { success = false, message = "Error updating teacher" });
            }
        }


        [HttpGet("GetTeacherIdByUserId")]
        public async Task<IActionResult> GetTeacherIdByUserId()
        {
            int UserId = (int)HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine("hello" + UserId);
            int result = await _teacherService.GetTeacherIdByuserId(UserId);
            if (result == 0)
            {
                return BadRequest("Not Found Teacher Id");
            }
            return Ok(result);
        }
        #endregion
    }
}
