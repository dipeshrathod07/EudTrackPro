using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Repositories.Implementations;
using Repositories.Interfaces;
using Repositories.Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TSController : ControllerBase
    {

        private readonly ITeacherInterface _teacherRepo;
        private readonly SubjectRepository _subjectRepo;
        private readonly TeacherSubjectRepository _teacherSubjectRepo;

        public TSController(ITeacherInterface tr, SubjectRepository sr, TeacherSubjectRepository tsr)
        {
            _teacherRepo = tr;
            _subjectRepo = sr;
            _teacherSubjectRepo = tsr;
        }


        /// <summary>
        ///Get all teachers with their IDs and names
        /// </summary>

        [HttpGet("get-teachers")]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = await _teacherRepo.GetTeacher();
            return Ok(teachers);
        }

        /// <summary>
        ///  Get all subjects with their IDs and names
        /// </summary>
        [HttpGet("get-subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var subjects = await _subjectRepo.GetStudent(); // Fix method name to GetSubjects if needed
            return Ok(subjects);
        }

        /// <summary>
        ///  Assign a subject to a teacher
        /// </summary>

        [HttpPost("assign-subject")]
        public async Task<IActionResult> AssignSubject([FromBody] t_assignSubject request)
        {
            if (request.TeacherId <= 0 || request.SubjectId <= 0)
            {
                return BadRequest(new { message = "Invalid Teacher or Subject ID" });
            }

            int result = await _teacherSubjectRepo.Add(request);

            if (result > 0)
                return Ok(new { message = "Subject assigned successfully!" });
            else
                return BadRequest(new { message = "Failed to assign subject." });
        }
    }
}
