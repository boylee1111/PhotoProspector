using PhotoProspector.Models;
using PhotoProspector.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Web;
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

            string result = SyncRequest(imdata);

            progress = 40f;


            if (result == "[]")
            {
                string[] noname = { "No face Detected!" };
                int[] noface = { 0, 0, 0, 0 };

                DrawImg(filepath, drawfilepath, noface, noname, 1);


            }
            else
            {

                string[] substr = StringSplit(result, "},{");
                string res = "";
                int facenum = substr.Length;
                int[] intarray = new int[4 * facenum];
                string[] alias = new string[substr.Length];
                string faceid = "";

                for (int i = 0; i < substr.Length; i++)
                {

                    res = substr[i];

                    faceid = getFaceID(res, "faceId");

                    left = getValue(res, "left");
                    top = getValue(res, "top");
                    width = getValue(res, "width");
                    height = getValue(res, "height");

                    intarray[4 * i] = left;
                    intarray[4 * i + 1] = top;
                    intarray[4 * i + 2] = width;
                    intarray[4 * i + 3] = height;

                    alias[i] = GetMatchAlias(faceid, trainingpath);

                }

                progress = 80f;

                DataSet[] dsarray = GetDataSetsByAlias(alias);
                string[] names = GetDisplayNameByDsarray(dsarray);
                DataSet myds = MergeDataSet(dsarray);
                DrawImg(filepath, drawfilepath, intarray, names, facenum);

                personlist.Persons = GetPersonListByDataSet(myds);
                personlist.ImageURL = "/images/" + drawfilename;

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




        //*********************************Action functions*********************************
        public List<Person> GetPersonListByDataSet(DataSet myDs)
        {

            List<Person> PersonList = new List<Person>();

            foreach (DataRow mDr in myDs.Tables[0].Rows)
            {
                Person person = new Person();

                foreach (DataColumn mDc in myDs.Tables[0].Columns)
                {
                    if (mDc.ColumnName == "DisplayName")
                    {
                        person.displayname = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "Alias")
                    {
                        person.alias = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "Title")
                    {
                        person.title = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "Team")
                    {
                        person.team = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "Specialty")
                    {
                        person.specialty = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "FavoriteSport")
                    {
                        person.favoritesport = mDr[mDc].ToString();
                    }
                    if (mDc.ColumnName == "photoPath")
                    {
                        person.photoPath = mDr[mDc].ToString();
                    }

                }

                PersonList.Add(person);

            }
            return PersonList;
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
        public string[] GetDisplayNameByDsarray(DataSet[] dsarray)
        {

            string[] displayname = new string[dsarray.Length];
            string tempname = "";

            for (int k = 0; k < dsarray.Length; k++)
            {
                if (dsarray[k].Tables[0].Rows.Count == 0)
                {
                    displayname[k] = "No Match";
                    continue;
                }


                foreach (DataRow mDr in dsarray[k].Tables[0].Rows)
                {
                    foreach (DataColumn mDc in dsarray[k].Tables[0].Columns)
                    {
                        if (mDc.ColumnName == "DisplayName")
                        {
                            tempname = mDr[mDc].ToString();
                            //Response.Write(tempname);                     
                            displayname[k] = tempname;


                        }
                    }
                }
            }

            //Response.Write(displayname.Length);

            return displayname;

        }
        public DataSet[] GetDataSetsByAlias(string[] str)
        {

            DataSet[] dsarray = new DataSet[str.Length];



            string myStr = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            SqlConnection myConn = new SqlConnection(myStr);
            myConn.Open();


            string query = "select * from FaceWebsiteTable WHERE alias='";

            for (int i = 0; i < str.Length; i++)
            {
                query += str[i] + "'";
                DataSet myDs = new DataSet();
                SqlDataAdapter myDa = new SqlDataAdapter(query, myConn);

                myDa.Fill(myDs);

                dsarray[i] = myDs;
                myDa.Dispose();
                myDs.Dispose();
                query = "select * from FaceWebsiteTable WHERE alias='";
            }


            myConn.Close();
            return dsarray;

        }
        public string GetMatchAlias(string faceid, string trainingpath)
        {

            string[] sourceid = System.IO.File.ReadAllLines(trainingpath + "faceids.txt");
            string[] sourcename = System.IO.File.ReadAllLines(trainingpath + "names.txt");
            string name = "";

            string resp;
            for (int i = 0; i < sourceid.Length; i++)
            {

                resp = VerifyRequest(faceid, sourceid[i]);
                if (resp.IndexOf("true") == -1)
                {

                    name = "No Match";
                    continue;

                }
                else
                {
                    name = sourcename[i];
                    break;

                }

            }

            string alias = name.Split('.')[0];

            return alias;

        }
        public string getFaceID(string str, string scanstr)
        {


            int inputl = scanstr.Length;
            int topindex = str.IndexOf(scanstr);

            char[] carray = str.ToCharArray();

            int i = topindex + inputl + 3;
            int j = 0;
            char[] topca = new char[100];


            while (carray[i] != '"' && j < 100)
            {
                topca[j] = carray[i];
                j++;
                i++;
            }

            string topstring = new string(topca);

            return topstring.Replace("\0", string.Empty);
        }
        public string[] StringSplit(string strSource, string strSplit)
        {
            string[] strtmp = new string[1];
            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[0] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[0] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }
        public string[] StringSplit(string strSource, string strSplit, string[] attachArray)
        {
            string[] strtmp = new string[attachArray.Length + 1];
            attachArray.CopyTo(strtmp, 0);

            int index = strSource.IndexOf(strSplit, 0);
            if (index < 0)
            {
                strtmp[attachArray.Length] = strSource;
                return strtmp;
            }
            else
            {
                strtmp[attachArray.Length] = strSource.Substring(0, index);
                return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
            }
        }
        public string SyncRequest(byte[] byteData)
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
        public static void DrawImg(string path, string newpath, int[] intarray, string[] namearray, int facenum)
        {


            System.Drawing.Image img = new Bitmap(path);
            Graphics g = Graphics.FromImage(img);


            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;

            if (namearray.Length != facenum)
            {
                namearray = new string[facenum];
            }

            for (int i = 0; i < facenum; i++)
            {

                Pen pen = new Pen(Color.DarkGreen, 2);
                Brush b = Brushes.DarkGreen;

                if (namearray[i] == "No Match" | namearray[i] == "No face Detected!")
                {
                    pen = new Pen(Color.Red, 2);
                    b = Brushes.Red;

                }


                x = intarray[4 * i];
                y = intarray[4 * i + 1];
                w = intarray[4 * i + 2];
                h = intarray[4 * i + 3];

                int fontrate = 4;
                int fontsize = (h * 72) / (fontrate * 96);

                if (x + y + w + h == 0)
                {
                    fontsize = 50;
                }
                g.DrawRectangle(pen, x, y, w, h);

                int FY = (y + h);

                g.DrawString(namearray[i], new Font("calibri", fontsize), b, new PointF(x, FY));

            }



            try
            {
                img.Save(newpath, System.Drawing.Imaging.ImageFormat.Jpeg);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                img.Dispose();
                g.Dispose();

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
        public string VerifyRequest(string faceid1, string faceid2)
        {

            string content = "{'faceId1':'" + faceid1 + "','faceId2':'" + faceid2 + "'}";
            //string content = "{'faceId1':'ffd471b8-8b18-4fca-a181-ad76c74c8915','faceId2':'c06a52b3-ce85-4a42-a68e-e4aafc9baf1b'}";


            WebClient client = new WebClient();

            var queryString = HttpUtility.ParseQueryString(string.Empty);

            try
            {
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("Ocp-Apim-Subscription-Key", "df8eaaf7a89f4d7885cbb02890296f1d");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string response;

            var uri = "https://api.projectoxford.ai/face/v1.0/verify?" + queryString;

            try
            {
                response = client.UploadString(uri, content);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }






    }
}