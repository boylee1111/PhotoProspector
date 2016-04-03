using PhotoProspector.Domain.Repository;
using System.Web.Mvc;

namespace PhotoProspector.Controllers
{
    public class HomeController : Controller
    {
        private IPersonRepository _personRepository;

        public HomeController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public HomeController()
        {

        }

        public ActionResult Index()
        {
            var persons = _personRepository.GetAll();
            foreach (var person in persons)
            {
                var name = person.FirstName + " " + person.LastName;
                var photos = person.Photos;
                foreach (var photo in photos)
                {
                    var date = photo.Date;
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}