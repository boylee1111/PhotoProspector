using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using PhotoProspector.Models;
using PhotoProspector.Services;
using PhotoProspector.ViewModels;
using System.Net;
using System.Web;
using System.Linq;

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
                    if (InsertUserToSQL(savedFullPaht, signUpViewModel))
                    {
                        string trainpath = Server.MapPath("~/ImageSource/");
                        StartTraining(trainpath);
                        return View(new SignUpViewModel(SignUpViewModel.SignUpStatus.SignUpSucceed));
                    }
                    else
                    {
                        return View(new SignUpViewModel(SignUpViewModel.SignUpStatus.SignUpFailed, "Sign up failed. This alias had been signed up."));
                    }
                }
            }
            catch
            {
                return View(new SignUpViewModel(SignUpViewModel.SignUpStatus.SignUpFailed, "Sign up failed. Please try again later."));
            }

            return View(signUpViewModel);
        }

        [HttpGet]
        public ActionResult DeleteUser()
        {
            return View(new DeleteUserViewModel());
        }

        [HttpPost]
        public ActionResult DeleteUser(DeleteUserViewModel deleteUserViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (DeleteUserFromSQL(deleteUserViewModel.alias))
                    {
                        return View(new DeleteUserViewModel(DeleteUserViewModel.DeleteUserStatus.DeleteUserSucceed));
                    }
                    else
                    {
                        return View(new DeleteUserViewModel(DeleteUserViewModel.DeleteUserStatus.DeleteUserFailed, "Delete user failed. User alias doesn't exist."));
                    }
                }
            }
            catch
            {
                return View(new DeleteUserViewModel(DeleteUserViewModel.DeleteUserStatus.DeleteUserFailed, "Delete user failed. Please try again later."));
            }

            return View(deleteUserViewModel);
        }


        public void StartTraining(string trainpath)
        {
            

            string logfile = trainpath + "trainingLog.txt";

            FileStream logfs;

            if (!System.IO.File.Exists(logfile))
            {
                logfs = System.IO.File.Create(logfile);
            }
            else
            {

                logfs = new FileStream(logfile, FileMode.Append);

            }

            System.IO.StreamWriter sw = new StreamWriter(logfs);


            try
            {

                string[] filepaths = Directory.GetFiles(trainpath, "*.jpg");

                string[] filename = new string[filepaths.Length];

                string[] temp;

                string[] faceids = new string[filepaths.Length];

                for (int i = 0; i < filepaths.Length; i++)
                {

                    temp = filepaths[i].Split('\\');
                    filename[i] = temp.Last();

                    FileStream fs = new FileStream(filepaths[i], FileMode.Open, FileAccess.ReadWrite);
                    byte[] b = new byte[fs.Length];
                    fs.Read(b, 0, (int)fs.Length);
                    fs.Close();

                    string response = SyncRequest(b);

                    faceids[i] = getFaceID(response, "faceId");

                }


                string nametxtpath = trainpath + "\\" + "names.txt";
                string facetxtpath = trainpath + "\\" + "faceids.txt";

                System.IO.File.WriteAllLines(nametxtpath, filename);
                System.IO.File.WriteAllLines(facetxtpath, faceids);

                string dt = System.DateTime.Now.ToString();
                string goodlog = dt + "-Training OK!";
                sw.WriteLine(goodlog);

                sw.Close();
                logfs.Close();

            }

            catch (Exception ex)
            {

                string dt = System.DateTime.Now.ToString();
                string badlog = dt + "-Training Failed!-Error:" + ex.Message;
                sw.WriteLine(badlog);

                sw.Close();
                logfs.Close();

            }

        }
        public bool DeleteUserFromSQL(string alias)
        {
#if DEBUG
            return (new Random().Next(100) % 2 == 0);
#endif

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
#if DEBUG
            return (new Random().Next(100) % 2 == 0);
#endif
            bool result = false;

            string[] temp = imagepath.Split('.');
            string extend = temp[temp.Length - 1];

            string cutname = rperson.alias + "." + extend;
            string cutpath = Server.MapPath("~/ImageSource/") + cutname;

            rperson.photoPath = ".\\ImageSource\\" + cutname;


            if(rperson.alias==null)
            {
                rperson.alias = "";
            }
            if (rperson.displayname == null)
            {
                rperson.displayname = "";
            }
            if (rperson.title == null)
            {
                rperson.title = "";
            }
            if (rperson.team == null)
            {
                rperson.team = "";
            }
            if (rperson.specialty == null)
            {
                rperson.specialty = "";
            }
            if (rperson.favoritesport == null)
            {
                rperson.favoritesport = "";
            }

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
        public static string SyncRequest(byte[] byteData)
        {

            WebClient client = new WebClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            try
            {
                client.Headers.Add("Content-Type", "application/octet-stream");
                client.Headers.Add("Ocp-Apim-Subscription-Key", "df8eaaf7a89f4d7885cbb02890296f1d");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Request parameters
            queryString["returnFaceId"] = "true";
            queryString["returnFaceLandmarks"] = "false";
            //queryString["returnFaceAttributes"] = "{string}";
            var uri = "https://api.projectoxford.ai/face/v1.0/detect?" + queryString;



            byte[] response = client.UploadData(uri, "POST", byteData);

            string result = System.Text.Encoding.UTF8.GetString(response);

            return result;

        }
        public static string getFaceID(string str, string scanstr)
        {


            int inputl = scanstr.Length;
            int topindex = str.IndexOf(scanstr);

            char[] carray = str.ToCharArray();

            int i = topindex + inputl + 3;
            int j = 0;
            char[] topca = new char[carray.Length];


            while (carray[i] != '"' && j < carray.Length)
            {
                topca[j] = carray[i];
                j++;
                i++;
            }

            string topstring = new string(topca);

            return topstring.Replace("\0", string.Empty);
        }


    }


}