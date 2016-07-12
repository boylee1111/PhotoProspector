using System;
using System.Net;
using System.Web;

namespace PhotoProspector.Services
{
    public interface IScanningService
    {
        string GetMatchAlias(string faceid, string trainingpath);
        string getFaceID(string str, string scanstr);
    }

    class ScanningService : IScanningService
    {
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

        public string GetMatchAlias(string faceid, string trainingpath)
        {
            string[] sourceid = System.IO.File.ReadAllLines(trainingpath + "faceids.txt");
            string[] sourcename = System.IO.File.ReadAllLines(trainingpath + "names.txt");
            string name = "";

            string resp;
            for (int i = 0; i < sourceid.Length; i++)
            {
                resp = VerifyRequest(faceid, sourceid[i]);

                string[] tempth = resp.Split(':');
                string[] tempth2 = tempth[2].Split('}');
                float value = Convert.ToSingle(tempth2[0]);
                float threshold = 0.50f;

                if (resp.IndexOf("true") == -1 || value < threshold)
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


            string alias = System.IO.Path.GetFileNameWithoutExtension(name);

            return alias;
        }

        private string VerifyRequest(string faceid1, string faceid2)
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
