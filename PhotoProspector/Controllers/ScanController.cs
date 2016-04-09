using PhotoProspector.Models;
using PhotoProspector.ViewModels;
using System.Threading;
using System.Web.Mvc;

namespace PhotoProspector.Controllers
{
    public class ScanController : Controller
    {
        private static double progress = 0.00f;

        [HttpPost]
        public ActionResult Index(string fileName)
        {
            ViewBag.fileName = fileName;
            return View();
        }

        [HttpPost]
        public ActionResult Scan(string filePath)
        {
            progress = 0.00f;
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(30);
                progress = i;
            }
            progress = 100.00f;
            return PartialView(fakeViewModel());
        }

        [HttpPost]
        public ActionResult ScanProgress()
        {
            return Json(string.Format("{0:0.##\\%}", progress));
        }

        public PersonListViewModel fakeViewModel()
        {
            var personListViewModel = new PersonListViewModel();

            var person1 = new Person();
            person1.displayname = "Dwarf One Dwarf One Dwarf One Dwarf One Dwarf One Dwarf One Dwarf One Dwarf One Dwarf One Dwarf One Dwarf One";
            person1.alias = "one";
            person1.title = "Athelete";
            person1.specialty = "Strength";
            person1.team = "Team 1";
            person1.favoritesport = "Weightlifting";
            person1.photoPath = "/Content/Images/test_img_1.png";
            var person2 = new Person();
            person2.displayname = "Dwarf Two";
            person2.alias = "two";
            person2.title = "Safeguard";
            person2.specialty = "Clairvoyance";
            person2.team = "Team 2";
            person2.favoritesport = "Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change";
            person2.photoPath = "/Content/Images/test_img_2.png";
            var person3 = new Person();
            person3.displayname = "Dwarf Three";
            person3.alias = "three";
            person3.title = "Tank";
            person3.specialty = "Impregnable";
            person3.team = "Team 3";
            person3.favoritesport = "";
            person3.photoPath = "/Content/Images/test_img_3.png";

            personListViewModel.Persons.Add(person1);
            personListViewModel.Persons.Add(person2);
            personListViewModel.Persons.Add(person3);

            personListViewModel.ImageURL = "/Content/Images/test.jpg";

            return personListViewModel;

        }
    }
}