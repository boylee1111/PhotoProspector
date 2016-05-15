namespace PhotoProspector.Helpers
{
    public static class StringExtensions
    {
        public static string[] StringSplit(this string strSource, string strSplit)
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
                return (strSource.Substring(index + strSplit.Length)).StringSplit(strSplit, strtmp);
            }
        }

        public static string[] StringSplit(this string strSource, string strSplit, string[] attachArray)
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
                return (strSource.Substring(index + strSplit.Length)).StringSplit(strSplit, strtmp);
            }
        }
    }
}
