using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Edutrack;
using Edutrack.Models;
using System.Threading.Tasks;
namespace Edutrack.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {

        private readonly IRegisterInterface _registerService;
        private readonly ILoginInterface _logiService;


        public RegisterController(IRegisterInterface register, ILoginInterface logicService)
        {
            _registerService = register;
            _logiService = logicService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterPost([FromBody] t_Register registerData)
        {
            var result = await _registerService.RegisterRepo(registerData);
            if (result == 1)
            {
                return Ok(new { message = "done bro" });
            }
            else if (result == 2)
            {
                return BadRequest("Email already exists");  // need to implement in repo for duplicate email id
            }
            else
            {
                return BadRequest("Login Failed");
            }
        }

        // [HttpPost]
        // [Route("Login")]
        // public async Task<IActionResult> Login([FromBody] t_Login login)
        // {
        //     int userId = await _logiService.Login(login);

        //     if (userId > 0)
        //     {
        //         if (login.Role == "Teacher")
        //         {
        //             string status = await _logiService.CheckTeacherStatus(userId);

        //             if (status == "Approved")
        //             {
        //                 return new JsonResult(new { success = true, message = "Login successful!", redirectUrl = Url.Action("Index", "Login") });
        //             }
        //             else
        //             {
        //                 return new JsonResult(new { success = false, message = "Your status is still pending. Please wait for approval." });
        //             }
        //         }

        //         return new JsonResult(new { success = true, message = "Login successful!", redirectUrl = Url.Action("Index", "Login") });
        //     }
        //     else
        //     {
        //         return new JsonResult(new { success = false, message = "Username or password incorrect" });
        //     }
        // }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] t_Login login)
        {
            int userId = await _logiService.Login(login);
            if (userId > 0)
            {
                HttpContext.Session.SetInt32("UserId", userId);


                if (login.Role == "Teacher")
                {
                    string status = await _logiService.CheckTeacherStatus(userId);

                    if (status == "approved")
                    {
                        return new JsonResult(new { success = true, message = "Login successful!", redirectUrl = "/Teacher/UpdateTeacher" });
                    }
                    else
                    {
                        return new JsonResult(new { success = false, message = "Your status is still pending. Please wait for approval." });
                    }
                }

                if (login.Role == "Student")
                {
                    return new JsonResult(new { success = true, message = "Login successful!", redirectUrl = "/Student/StuDash" });
                }

                return new JsonResult(new { success = true, message = "Login successful!", redirectUrl = "/Admin/Index" });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Username or password incorrect" });
            }
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok("Logout Successful");
        }

    }
}
