using System.Web.Mvc;

namespace PhotoProspector.Controllers
{
    public class ScanController : Controller
    {
        [HttpPost]
        public ActionResult Index(string fileName)
        {
            ViewBag.fileName = fileName;
            return View();
        }
    }
}