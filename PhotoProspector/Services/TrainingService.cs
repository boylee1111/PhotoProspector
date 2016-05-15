using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace PhotoProspector.Services
{
    public interface ITrainingService
    {
        void StartTraining(string trainpath);
    }

    class TrainingService : ITrainingService
    {
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

        private static string SyncRequest(byte[] byteData)
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
        private static string getFaceID(string str, string scanstr)
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
