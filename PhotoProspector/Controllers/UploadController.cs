
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PhotoProspector.Controllers
{
    public class UploadController : Controller
    {
        private const int UploadScreenWidth = 800;  // Change the value of the width of the image on the screen

        private const string TempFolder = "/Temp";
        private const string MapTempFolder = "~" + TempFolder;
        private const string UploadPath = "/Uploads";

        private readonly string[] _imageFileExtensions = { ".jpg", ".png", ".gif", ".jpeg" };

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null || !files.Any())
            {
                return Json(new { success = false, errorMessage = "No file uploaded." });
            }
            var file = files.FirstOrDefault();  // get ONE only
            if (file == null || !IsImage(file))
            {
                return Json(new { success = false, errorMessage = "File is of wrong format." });
            }
            if (file.ContentLength <= 0)
            {
                return Json(new { success = false, errorMessage = "File cannot be zero length." });
            }
            var webPath = GetTempSavedFilePath(file);
            return Json(new { success = true, fileName = webPath.Replace("/", "\\") }); // success
        }

        [HttpPost]
        public ActionResult Save(string fileName)
        {
            try
            {
                // Get file from temporary folder
                var fn = Path.Combine(Server.MapPath(MapTempFolder), Path.GetFileName(fileName));
                var img = new WebImage(fn);
                // ... delete the temporary file,...
                System.IO.File.Delete(fn);
                // ... and save the new one.
                var newFileName = Path.Combine(UploadPath, Path.GetFileName(fn));
                var newFileLocation = HttpContext.Server.MapPath(newFileName);
                if (Directory.Exists(Path.GetDirectoryName(newFileLocation)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newFileLocation));
                }

                img.Save(newFileLocation);
                return Json(new { success = true, uploadFileLocation = newFileName });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Unable to upload file.\nERRORINFO: " + ex.Message });
            }
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return false;
            }
            return file.ContentType.Contains("image") ||
                _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        private string GetTempSavedFilePath(HttpPostedFileBase file)
        {
            // Define destination
            var serverPath = HttpContext.Server.MapPath(TempFolder);
            if (Directory.Exists(serverPath) == false)
            {
                Directory.CreateDirectory(serverPath);
            }

            // Generate unique file name
            var fileName = Path.GetFileName(file.FileName);
            fileName = SaveTemporaryUploadFileImage(file, serverPath, fileName);

            // Clean up old files after every save
            CleanUpTempFolder(1);
            return Path.Combine(TempFolder, fileName);
        }

        private static string SaveTemporaryUploadFileImage(HttpPostedFileBase file, string serverPath, string fileName)
        {
            var img = new WebImage(file.InputStream);
            var ratio = img.Height / (double)img.Width;
            img.Resize(UploadScreenWidth, (int)(UploadScreenWidth * ratio));

            var fullFileName = Path.Combine(serverPath, fileName);
            if (System.IO.File.Exists(fullFileName))
            {
                System.IO.File.Delete(fullFileName);
            }

            img.Save(fullFileName);
            return Path.GetFileName(img.FileName);
        }

        private void CleanUpTempFolder(int hoursOld)
        {
            try
            {
                var currentUtcNow = DateTime.UtcNow;
                var serverPath = HttpContext.Server.MapPath("/Temp");
                if (!Directory.Exists(serverPath)) return;
                var fileEntries = Directory.GetFiles(serverPath);
                foreach (var fileEntry in fileEntries)
                {
                    var fileCreationTime = System.IO.File.GetCreationTimeUtc(fileEntry);
                    var res = currentUtcNow - fileCreationTime;
                    if (res.TotalHours > hoursOld)
                    {
                        System.IO.File.Delete(fileEntry);
                    }
                }
            }
            catch
            {
                // Deliberately empty.
            }
        }
    }
}
