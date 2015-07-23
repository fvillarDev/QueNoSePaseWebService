using System.IO;
using System.Net;
using System.Text;

namespace QueNoSePaseWebService
{
    public class HttpHelper
    {
        public static string Post(string url, string param)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";
            //var postData = "{\"Lineas_Id\":20,\"Dias_Id\":1}";
            var bytes = Encoding.UTF8.GetBytes(param);
            request.ContentLength = bytes.Length;

            var requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);

            var result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();
            return result;
        }
    }
}