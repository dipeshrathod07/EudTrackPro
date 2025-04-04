using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    public class ExamController : Controller
    {
        // GET: ExamController
        public ActionResult Index()
        {
            return View();
        }
         public ActionResult AddExam()
        {
            return View();
        }

    }
}
