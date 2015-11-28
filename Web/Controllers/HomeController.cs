using System.Web.Mvc;

namespace CourseCentral.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
    }
}