using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;

    private readonly HttpClient _httpClient;

    public AdminController(ILogger<AdminController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
    }

    #region Home Controller
    public IActionResult Index()
    {
        return View();
    }

    #endregion

    #region class controller 

    public IActionResult ClassList()
    {
        return View();
    }

    #endregion

    #region notification controller 

    public IActionResult Add_Notification()
    {
        return View();
    }

    #endregion

    #region Section controller

    public IActionResult SectionList()
    {
        return View();
    }

    #endregion

    #region Student section

    public ActionResult StudentIndex()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult StudentList()
    {
        return View();
    }

    #endregion

    #region Teacher controller

    public ActionResult TeacherIndex()
    {
        return View();
    }
    public ActionResult List()
    {
        return View();
    }
    public ActionResult List2()
    {
        return View();
    }

    #endregion

    #region Teacher Subject controller

    public ActionResult TeacherSubjectIndex()
    {
        return View();
    }

    #endregion

    #region Exam controller
    public ActionResult AddExam()
    {
        return View();
    }
    #endregion

    public async Task<IActionResult> Logout()
    {
        string url = "http://localhost:5113/api/Register/logout";
        HttpResponseMessage message = await _httpClient.GetAsync(url);
        return RedirectToAction("Login", "Home");
    }
}