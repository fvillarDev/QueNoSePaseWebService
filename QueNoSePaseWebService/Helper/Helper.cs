using System.Configuration;
using System.IO;
using System.Net;

namespace QueNoSePaseWebService.Helper
{
    public class Helper
    {
        internal static bool Validate(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return false;
            return a.Equals(ConfigurationManager.AppSettings["User"]) || b.Equals(ConfigurationManager.AppSettings["Pass"]);
        }

        internal static string Request(string url)
        {
            string result;
            var req = WebRequest.Create(url);
            var resp = req.GetResponse();
            using (var stream = resp.GetResponseStream())
            {
                if (stream != null)
                    using (var sr = new StreamReader(stream))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                else return "ERROR";
            }

            return string.IsNullOrEmpty(result) ? "ERROR" : result;
        }
    }
}