using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult StuDash()
        {
            return View();
        }
        public async Task<ActionResult> Logout()
        {
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Feedback()
        {
            return View();
        }
        public IActionResult Timetable()
        {
            return View();
        }
        public IActionResult Material()
        {
            return View();
        }
    }
}