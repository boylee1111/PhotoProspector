using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using PhotoProspector.Models;
using PhotoProspector.Services;
using PhotoProspector.ViewModels;

namespace PhotoProspector.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IImageService imageService;
        private readonly IFileService fileService;

        public SignUpController(
            IImageService imageService,
            IFileService fileService)
        {
            this.imageService = imageService;
            this.fileService = fileService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new SignUpViewModel());
        }

        [HttpPost]
        public ActionResult Index(SignUpViewModel signUpViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(signUpViewModel.photoPath.FileName);
                    var tempFolder = Server.MapPath("~/TempImg/");
                    if (!Directory.Exists(tempFolder))
                    {
                        Directory.CreateDirectory(tempFolder);
                    }
                    var savedFullPaht = tempFolder + fileService.filenameHashed(filename);
                    signUpViewModel.photoPath.SaveAs(savedFullPaht);
                    InsertUserToSQL(savedFullPaht, signUpViewModel);
                }
            }
            catch
            {
                return View(signUpViewModel);
            }

            return View(signUpViewModel);
        }

        public bool DeleteUserFromSQL(string alias)
        {
            bool result = false;

            string trainimgpath1 = Server.MapPath("~/ImageSource/") + alias + ".jpg";
            string trainimgpath2 = Server.MapPath("~/ImageSource/") + alias + ".png";

            if (System.IO.File.Exists(trainimgpath1))
            {
                System.IO.File.Delete(trainimgpath1);
            }

            if (System.IO.File.Exists(trainimgpath2))
            {
                System.IO.File.Delete(trainimgpath2);
            }

            string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection myConn = new SqlConnection(myStr);
            myConn.Open();

            SqlCommand delete = new SqlCommand("Delete from FaceWebsiteTable Where Alias = '" + alias + "'", myConn);

            try
            {

                delete.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myConn.Close();
            }

            result = true;
            return result;
        }
        public bool InsertUserToSQL(string imagepath, Person rperson)
        {

            bool result = false;

            string[] temp = imagepath.Split('.');
            string extend = temp[temp.Length - 1];

            string cutname = rperson.alias + "." + extend;
            string cutpath = Server.MapPath("~/ImageSource/") + cutname;

            rperson.photoPath = "./ImageSource/" + cutname;

            this.imageService.CutImg(imagepath, cutpath, 800, 800, "HW");

            string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection myConn = new SqlConnection(myStr);
            myConn.Open();

            string query = "select * from FaceWebsiteTable WHERE alias='" + rperson.alias + "'";

            DataSet myDs = new DataSet();
            SqlDataAdapter myDa = new SqlDataAdapter(query, myConn);

            myDa.Fill(myDs);

            if (myDs.Tables[0].Rows.Count > 0)
            {

                result = false;
                return result;
            }
            else
            {
                SqlCommand insert = new SqlCommand("insert into FaceWebsiteTable(displayname, alias, team,title,specialty,favoritesport,photoPath) values(@displayname, @alias, @team, @title, @specialty, @favoritesport, @photoPath)", myConn);
                insert.Parameters.AddWithValue("@displayname", rperson.displayname);
                insert.Parameters.AddWithValue("@alias", rperson.alias);
                insert.Parameters.AddWithValue("@team", rperson.team);
                insert.Parameters.AddWithValue("@title", rperson.title);
                insert.Parameters.AddWithValue("@specialty", rperson.specialty);
                insert.Parameters.AddWithValue("@favoritesport", rperson.favoritesport);
                insert.Parameters.AddWithValue("@photoPath", rperson.photoPath);

                try
                {

                    insert.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;

                }
                finally
                {
                    myConn.Close();
                }
            }
            result = true;
            return result;
        }
    }


}