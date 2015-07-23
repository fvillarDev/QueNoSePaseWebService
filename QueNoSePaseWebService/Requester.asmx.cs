using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Hosting;
using System.Web.Services;
using System.Xml;
using Newtonsoft.Json;
using QueNoSePaseWebService.Helper;
using QueNoSePaseWebService.Models;
using QueNoSePaseWebService.Properties;

namespace QueNoSePaseWebService
{
    [WebService(Namespace = "http://quenosepase.com.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Requester : WebService
    {
        [WebMethod]
        public string GetPuestosRecarga(string a, string b, string c)
        {
            if (!Helper.Helper.Validate(a, b)) return "ERROR";

            try
            {
                var url = ConfigurationManager.AppSettings["RedBusCentroCargas"];
                if (!string.IsNullOrEmpty(c)) url += c;

                var result = Helper.Helper.Request(url);

                var doc = new XmlDocument();
                doc.LoadXml(result.Replace("<?xml version=\"1.0\"?>\n", ""));
                var json = JsonConvert.SerializeXmlNode(doc);

                return json;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //[WebMethod]
        public string GetRecorrido(string a, string b, string c, string d)
        {
            if (!Helper.Helper.Validate(a, b)) return "ERROR";

            try
            {
                string url;
                if (c.Equals("ERSA"))
                {
                    url = ConfigurationManager.AppSettings["KmlErsaUrl"];
                    url += d;
                    if (d.Contains("kmz"))
                    {
                        var zipFileName = HostingEnvironment.ApplicationPhysicalPath + Guid.NewGuid().ToString("N") + ".zip";
                        using (var client = new WebClient())
                        {
                            client.DownloadFile(new Uri(url), zipFileName);
                        }
                        var filePath = HostingEnvironment.ApplicationPhysicalPath;
                        var fileName = ZipHelper.ExtractZipFile(zipFileName, filePath);
                        var kml = File.ReadAllText(filePath + fileName);
                        var doc2 = new XmlDocument();
                        doc2.LoadXml(kml);
                        var tmp2 = Recorrido.ParseErsaKmzFromXml(doc2);
                        return JsonConvert.SerializeObject(tmp2);
                    }
                    var result = Helper.Helper.Request(url);
                    var doc = new XmlDocument();
                    doc.LoadXml(result);
                    var tmp = Recorrido.ParseErsaFromXml(doc);
                    return JsonConvert.SerializeObject(tmp);
                }
                return "ERROR";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //a:user, b:pass, c:linea, d:dia
        [WebMethod]
        public string GetFrecuencia(string a, string b, string c, string d)
        {
            if (!Helper.Helper.Validate(a, b)) return "ERROR";

            try
            {
                var url = ConfigurationManager.AppSettings["FrecuenciasUrl"];
                var param = Resources.PostFrecuenciasParams.Replace("[LINEA]", c).Replace("[DIA]", d);
                var response = HttpHelper.Post(url, param);
                var frecuencias = JsonConvert.DeserializeObject<Frecuencias>(response);
                var frecuenciasArr = JsonConvert.DeserializeObject<FrecuenciaLineaDias[]>(frecuencias.GetFrecuenciaByLineasDiasResult);
                return JsonConvert.SerializeObject(frecuenciasArr);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //a:user, b:pass, c:linea, d:origen, e:sentido
        [WebMethod]
        public string GetRecorrido(string a, string b, string c, string d, string e)
        {
            if (!Helper.Helper.Validate(a, b)) return "ERROR";

            try
            {
                var recorridoId = RecorridoHelper.GetRecorridoId(c, d, e);
                return RecorridoHelper.GetRecorridoDetalle(recorridoId);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
