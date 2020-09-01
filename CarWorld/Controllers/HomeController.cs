using System.Web.Mvc;

namespace CarWorld.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Car World - Test Project for Triangle Software";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Jason Lindsay";

            return View();
        }
    }
}