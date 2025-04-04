using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardApiController : ControllerBase
    {
     private readonly IStudentInterface _student;
       private readonly ITeacherInterface _teacher;
        private readonly IClassInterface _class;

     public DashboardApiController(IStudentInterface student,ITeacherInterface teacher,IClassInterface Class){
        _student=student;
        _teacher= teacher;
        _class=Class;

     }

     [HttpGet("counts")]
        public async Task<IActionResult> GetCounts()
        {
            var studentCount = await _student.GetStudentCount();
            var teacherCount = await _teacher.GetTeacherCount();
            var classCount = await _class.GetClassCount();

            return Ok(new
            {
                Students = studentCount,
               Teachers = teacherCount,
                Classes = classCount
            });
        }

    }
}
