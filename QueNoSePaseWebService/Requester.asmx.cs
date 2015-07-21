using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web.Services;
using System.Xml;
using Newtonsoft.Json;
using QueNoSePaseWebService.Models;

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

        [WebMethod]
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
                        return "CURRENTLY NOT AVAILABLE";
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
    }
}
