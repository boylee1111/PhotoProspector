
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        private const string UploadImgFolder = "/images";
        private const string UploadPath = "~" + UploadImgFolder;

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
                var fn = Path.Combine(Server.MapPath(UploadPath), Path.GetFileName(fileName));
                var img = new WebImage(fn);
                // ... delete the temporary file,...
                System.IO.File.Delete(fn);
                // ... and save the new one.
                var newFileName = Path.Combine(UploadPath, Path.GetFileName(fn));
                var newFileLocation = Server.MapPath(newFileName);
                if (Directory.Exists(Path.GetDirectoryName(newFileLocation)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(newFileLocation));
                }

                img.Save(newFileLocation);

                int w = 800;
                int h = 800;

                string filename = Path.GetFileName(fn);

                string[] arrays = filename.Split('.');
                string cutfilename = arrays[0] + "cut." + arrays[1];
                string cutfilepath = Server.MapPath("~/images/") + cutfilename;

                CutImg(newFileLocation, cutfilepath, w, h, "HW");

                if (System.IO.File.Exists(newFileLocation))
                {
                    System.IO.File.Delete(newFileLocation);
                }


                return Json(new { success = true, uploadFileLocation = "." + Path.Combine(UploadImgFolder, cutfilename) });
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
            var serverPath = Server.MapPath(UploadPath);
            if (Directory.Exists(serverPath) == false)
            {
                Directory.CreateDirectory(serverPath);
            }

            // Generate unique file name
            var fileName = Path.GetFileName(file.FileName);
            fileName = SaveTemporaryUploadFileImage(file, serverPath, fileName);

            // Clean up old files after every save
            CleanUpTempFolder(1);
            return Path.Combine(UploadPath, fileName);
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
                var serverPath = Server.MapPath("~/Temp");
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


        public static void CutImg(string oPath, string nPaht, int w, int h, string mode)
        {

            FileStream fs = new FileStream(oPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            System.Drawing.Image oimg = System.Drawing.Image.FromStream(fs);

            //System.Drawing.Image oimg = System.Drawing.Image.FromFile(oPath);

            int nToWidth = w;
            int nToHeight = h;
            int x = 0;
            int y = 0;
            int oWidth = oimg.Width;
            int oHeight = oimg.Height;
            switch (mode)
            {

                case "HW":
                    if (oimg.Width > oimg.Height)
                    {
                        nToHeight = nToWidth * oHeight / oWidth;
                    }
                    else
                    {
                        nToWidth = nToHeight * oWidth / oHeight;
                    }
                    break;
                case "W":
                    nToHeight = oWidth * oHeight / nToWidth;
                    break;
                case "H":
                    nToWidth = oWidth * oHeight / nToHeight;
                    break;
                case "CUT":
                    if ((oimg.Width / oimg.Height) > (nToWidth / nToHeight))
                    {
                        oHeight = oimg.Height;
                        oWidth = oimg.Height * nToWidth / nToHeight;
                        y = 0;
                        x = (oimg.Width - oWidth) / 2;
                    }
                    else
                    {
                        oWidth = oimg.Width;
                        oHeight = oimg.Width * nToHeight / nToWidth;
                        x = 0;
                        y = (oimg.Height - oHeight) / 2;
                    }
                    break;
                default: break;
            }

            System.Drawing.Image bitmap = new Bitmap(nToWidth, nToHeight);

            Graphics gp = Graphics.FromImage(bitmap);
            gp.InterpolationMode = InterpolationMode.High;
            gp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            gp.Clear(Color.Transparent);
            gp.DrawImage(oimg, new Rectangle(0, 0, nToWidth, nToHeight), new Rectangle(x, y, oWidth, oHeight), GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(nPaht, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {

                oimg.Dispose();
                bitmap.Dispose();
                fs.Close();

            }
        }



    }


}
