using System;
using System.Configuration;
using System.IO;
using System.Net;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using QueNoSePaseWebService.Models;
using QueNoSePaseWebService.Properties;

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

    public class ZipHelper
    {
        internal MemoryStream CreateToMemoryStream(MemoryStream memStreamIn, string zipEntryName)
        {
            var outputMemStream = new MemoryStream();
            var zipStream = new ZipOutputStream(outputMemStream);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

            var newEntry = new ZipEntry(zipEntryName);
            newEntry.DateTime = DateTime.Now;

            zipStream.PutNextEntry(newEntry);

            StreamUtils.Copy(memStreamIn, zipStream, new byte[4096]);
            zipStream.CloseEntry();

            zipStream.IsStreamOwner = false;    // False stops the Close also Closing the underlying stream.
            zipStream.Close();          // Must finish the ZipOutputStream before using outputMemStream.

            outputMemStream.Position = 0;
            return outputMemStream;
        }

        internal static string ExtractZipFile(string zipFilePath, string outFolder)
        {
            ZipFile zf = null;
            var entryFileName = Guid.NewGuid().ToString("N") + ".kml";//zipEntry.Name;
            try
            {
                var fs = File.OpenRead(zipFilePath);
                zf = new ZipFile(fs);
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;
                    }
                    
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    var buffer = new byte[4096];     // 4K is optimum
                    var zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    var fullZipToPath = Path.Combine(outFolder, entryFileName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
            return entryFileName;
        }
    }

    public class RecorridoHelper
    {
        public static string GetRecorridoId(string c, string d, string e)
        {
            var url = ConfigurationManager.AppSettings["RecorridosActuales"];
            var param = Resources.PostRecorridoActualesParams.Replace("[LINEA]", c).Replace("[ORIGEN]", d);
            var response = HttpHelper.Post(url, param);
            var recorridosActualesStr = JsonConvert.DeserializeObject<Recorrido2>(response);
            var recorridosActualesArr = JsonConvert.DeserializeObject<Recorrido2Data[]>(recorridosActualesStr.RecorridosActualesBuscarResult);
            var recorridoId = "";
            foreach (var data in recorridosActualesArr)
            {
                if (data.sentido != e) continue;
                recorridoId = data.recorridos_id;
                break;
            }
            return recorridoId;
        }

        public static string GetRecorridoDetalle(string id)
        {
            var url = ConfigurationManager.AppSettings["ObtenerRecorrido"];
            var param = Resources.PostRecorridoDetalleParams.Replace("[ID]", id);
            var response = HttpHelper.Post(url, param);
            var recorridosDetalleStr = JsonConvert.DeserializeObject<RecorridoDetalleResult>(response);
            var recorridosDetalleArr = JsonConvert.DeserializeObject<RecorridoDetalle[]>(recorridosDetalleStr.GetByRecorridosIdResult);
            return JsonConvert.SerializeObject(recorridosDetalleArr); ;
        }
    }
}