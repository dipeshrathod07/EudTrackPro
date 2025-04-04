using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class TeacherController : Controller
{
    private readonly ILogger<TeacherController> _logger;

    public TeacherController(ILogger<TeacherController> logger)
    {
        _logger = logger;
    }

    #region Schedular & Update

    public ActionResult Scheduler()
    {
        return View();
    }

    public IActionResult UpdateTeacher()
    {
        return View();
    }
    #endregion

    #region viral naitik Dashboard

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult UploadMaterials()
    {
        return View();
    }

    public IActionResult AllMaterials()
    {
        return View();
    }

    public IActionResult Classes() => View();
    public IActionResult Analytics() => View();
    public IActionResult Notifications() => View();


    private readonly string _materialsPath = "wwwroot/UploadedMaterials/";

    [HttpGet]
    public IActionResult ViewFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("File name is required.");
        }

        // ✅ Build full file path
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), _materialsPath, fileName);

        // ✅ Check if the file exists
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found.");
        }

        // ✅ Serve the file
        return PhysicalFile(filePath, "application/pdf");
    }
    #endregion




}
