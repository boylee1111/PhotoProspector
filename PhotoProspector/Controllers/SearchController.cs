using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Helpers;
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
                var savedDirectoryRootPath = SearchResultImagePath + alias + "/";
                var savedDirectoryPath = Server.MapPath("~" + savedDirectoryRootPath);

                // Save File
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(file);

                        if (Directory.Exists(savedDirectoryPath) == false)
                        {
                            Directory.CreateDirectory(savedDirectoryPath);
                        }
                        var img = new WebImage(fileContent.InputStream);
                        var ratio = img.Height / (double)img.Width;
                        if (img.Width > UploadScreenWidth)
                        {
                            img.Resize(UploadScreenWidth, (int)(UploadScreenWidth * ratio));
                        }
                        if (img.Height > UploadScreenHeight)
                        {
                            img.Resize((int)(UploadScreenHeight / ratio), UploadScreenHeight);
                        }

                        var fullFileName = Path.Combine(savedDirectoryPath, filename);
                        if (System.IO.File.Exists(fullFileName))
                        {
                            System.IO.File.Decrypt(fullFileName);
                        }

                        img.Save(fullFileName);
                    }
                }

                if (!Directory.Exists(savedDirectoryPath))
                {
                    throw new IOException();
                }

                // Create ViewModel
                var imageFiles = new List<ImageFileViewModel>();
                Directory.GetFiles(savedDirectoryPath).ToList().ForEach(f =>
                {
                    var fileNameWithoutPath = Path.GetFileName(f);
                    var imagePathUrl = savedDirectoryRootPath + fileNameWithoutPath;
                    var imageFileModel = new ImageFileViewModel(imagePathUrl, fileNameWithoutPath);
                    imageFiles.Add(imageFileModel);
                });

                var searchResultViewModel = new SearchResultViewModel();
                searchResultViewModel.Alias = alias;
                searchResultViewModel.Images = imageFiles;

                return PartialView(searchResultViewModel);
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                throw new Exception();
            }
        }
    }
}