using System;
using System.Net;
using System.Web.Mvc;
using PhotoProspector.ViewModels;

namespace PhotoProspector.Controllers
{
    public class SearchController : Controller
    {
        private const int UploadScreenWidth = 600;  // Change the value of the width of the image on the screen
        private const int UploadScreenHeight = 900;
        private const string SearchResultImagePath = "/ImageSource/SearchImage/";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadBatchSearchByAlias(string alias)
        {
            try
            {
                var savedDirectoryRootPath = SearchResultImagePath + alias + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "/";
                savedDirectoryRootPath = SearchResultImagePath + "test/";
                var savedDirectoryPath = Server.MapPath("~" + savedDirectoryRootPath);

                var uploadPath = Server.MapPath("~" + SearchResultImagePath + "upload/");

                // Save File
                //foreach (string file in Request.Files)
                //{
                //    var fileContent = Request.Files[file];
                //    if (fileContent != null && fileContent.ContentLength > 0)
                //    {
                //        var filename = Path.GetFileName(file);

                //        if (Directory.Exists(uploadPath) == false)
                //        {
                //            Directory.CreateDirectory(uploadPath);
                //        }
                //        var img = new WebImage(fileContent.InputStream);
                //        var ratio = img.Height / (double)img.Width;
                //        if (img.Width > UploadScreenWidth)
                //        {
                //            img.Resize(UploadScreenWidth, (int)(UploadScreenWidth * ratio));
                //        }
                //        if (img.Height > UploadScreenHeight)
                //        {
                //            img.Resize((int)(UploadScreenHeight / ratio), UploadScreenHeight);
                //        }

                //        var fullFileName = Path.Combine(uploadPath, filename);
                //        if (System.IO.File.Exists(fullFileName))
                //        {
                //            System.IO.File.Decrypt(fullFileName);
                //        }

                //        img.Save(fullFileName);
                //    }
                //}

                var searchResultViewModel = new SearchResultViewModel(alias, savedDirectoryPath, savedDirectoryRootPath);

                return PartialView("SearchResultByAlias", searchResultViewModel);
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                throw new Exception();
            }
        }
    }
}