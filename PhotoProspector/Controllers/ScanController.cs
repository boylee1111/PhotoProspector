using System;
using System.Data;
using System.IO;
using System.Web.Mvc;
using PhotoProspector.Helpers;
using PhotoProspector.Models;
using PhotoProspector.Services;
using PhotoProspector.ViewModels;

namespace PhotoProspector.Controllers
{
    public class ScanController : Controller
    {
        private static double progress = 0.00f;

        private readonly IUserService userService;
        private readonly IImageService imageService;
        private readonly IScanningService scanningService;
        private readonly ITrainingService trainingService;

        public ScanController(
            IUserService userService,
            IImageService imageService,
            IScanningService scanningService,
            ITrainingService trainingService)
        {
            this.userService = userService;
            this.imageService = imageService;
            this.scanningService = scanningService;
            this.trainingService = trainingService;
        }

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

            PersonListViewModel personlist = new PersonListViewModel();

#if DEBUG
            personlist = fakeViewModel();
            progress = 100;
            return PartialView(personlist);
#endif

            string trainingpath = Server.MapPath("~/ImageSource/");
            //string trainingpath = "C:\\ASPNET\\FaceWebSite\\ImageSource\\";
            string[] array = filePath.Split('\\');
            string filename = array[array.Length - 1];
            string filepath = Server.MapPath("~/images/") + filename;

            string[] arrays2 = filename.Split('.');
            string drawfilename = arrays2[0] + "draw." + arrays2[1];
            string drawfilepath = Server.MapPath("~/images/") + drawfilename;

            progress = 10f;

            int top = 0;
            int left = 0;
            int width = 0;
            int height = 0;

            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite);
            byte[] imdata = new byte[fs.Length];
            fs.Read(imdata, 0, (int)fs.Length);
            fs.Close();

            progress = 25f;

            string result = trainingService.SyncRequest(imdata);

            progress = 40f;

            if (result == "[]")
            {
                string[] noname = { "No face Detected!" };
                int[] noface = { 0, 0, 0, 0 };

                imageService.DrawImg(filepath, drawfilepath, noface, noname, 1);

                personlist.ImageURL = "./images/" + drawfilename;
            }
            else
            {
                string[] substr = result.StringSplit("},{");
                string res = "";
                int facenum = substr.Length;
                int[] intarray = new int[4 * facenum];
                string[] alias = new string[substr.Length];
                string faceid = "";

                for (int i = 0; i < substr.Length; i++)
                {

                    res = substr[i];

                    faceid = scanningService.getFaceID(res, "faceId");

                    left = getValue(res, "left");
                    top = getValue(res, "top");
                    width = getValue(res, "width");
                    height = getValue(res, "height");

                    intarray[4 * i] = left;
                    intarray[4 * i + 1] = top;
                    intarray[4 * i + 2] = width;
                    intarray[4 * i + 3] = height;

                    alias[i] = scanningService.GetMatchAlias(faceid, trainingpath);
                }

                progress = 80f;

                DataSet[] dsarray = userService.GetDataSetsByAlias(alias);
                string[] names = userService.GetDisplayNameByDsarray(dsarray);
                DataSet myds = MergeDataSet(dsarray);
                imageService.DrawImg(filepath, drawfilepath, intarray, names, facenum);

                personlist.Persons = userService.GetPersonListByDataSet(myds);
                personlist.ImageURL = "./images/" + drawfilename;
            }

            progress = 100f;

            return PartialView(personlist);
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
            person1.photoPath = "./Content/Images/test_img_1.png";
            var person2 = new Person();
            person2.displayname = "Dwarf Two";
            person2.alias = "two";
            person2.title = "Safeguard";
            person2.specialty = "Clairvoyance";
            person2.team = "Team 2";
            person2.favoritesport = "Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change Hao Change";
            person2.photoPath = "./Content/Images/test_img_2.png";
            var person3 = new Person();
            person3.displayname = "Dwarf Three";
            person3.alias = "three";
            person3.title = "Tank";
            person3.specialty = "Impregnable";
            person3.team = "Team 3";
            person3.favoritesport = "";
            person3.photoPath = "./Content/Images/test_img_3.png";

            personListViewModel.Persons.Add(person1);
            personListViewModel.Persons.Add(person2);
            personListViewModel.Persons.Add(person3);

            personListViewModel.ImageURL = "./Content/Images/test.jpg";

            return personListViewModel;

        }

        public DataSet MergeDataSet(DataSet[] dsarray)
        {
            DataSet myds = new DataSet();

            for (int i = 0; i < dsarray.Length; i++)
            {
                myds.Merge(dsarray[i]);
            }

            return myds;
        }

        public int getValue(string str, string scanstr)
        {
            int inputl = scanstr.Length;
            int topindex = str.IndexOf(scanstr);

            char[] carray = str.ToCharArray();

            int i = topindex + inputl + 2;
            int j = 0;
            char[] topca = new char[10];

            while (carray[i] != ',' && carray[i] != '}' && j < 10)
            {
                topca[j] = carray[i];
                j++;
                i++;
            }

            string topstring = new string(topca);

            int topvalue = Convert.ToInt32(topstring);
            return topvalue;
        }
    }
}