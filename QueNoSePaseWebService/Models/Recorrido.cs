using System.Xml;

namespace QueNoSePaseWebService.Models
{
    public class Recorrido
    {
        public string Linea { get; set; }
        public string CoordenadasIda { get; set; }
        public string CoordenadasVuelta { get; set; }
        public string PunteroInicio { get; set; }
        public string PunteroFinal { get; set; }
        public string PunteroInicioCoordenada { get; set; }
        public string PunteroFinalCoordenada { get; set; }

        public static Recorrido ParseErsaFromXml(XmlDocument doc)
        {
            if (doc == null || doc.DocumentElement == null) return null;
            var manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("e", doc.DocumentElement.NamespaceURI);
            var placemarks = doc.DocumentElement.FirstChild.SelectNodes("e:Placemark", manager);
            string ida = null, vuelta = null, idaCoor = null, vueltaCoord = null, idaPlace = null, vueltaPlace = null;
            foreach (XmlNode node in placemarks)
            {
                if (node.SelectSingleNode("e:name", manager).InnerText.Equals("Ida"))
                    idaCoor = node.SelectSingleNode("e:LineString", manager).InnerText;
                else if (node.SelectSingleNode("e:name", manager).InnerText.Equals("Regreso"))
                    vueltaCoord = node.SelectSingleNode("e:LineString", manager).InnerText;
                else if (node.SelectSingleNode("e:name", manager).InnerText.Contains("Inicio"))
                {
                    ida = node.SelectSingleNode("e:name", manager).InnerText;
                    idaPlace = node.SelectSingleNode("e:Point", manager).InnerText;
                }
                else if (node.SelectSingleNode("e:name", manager).InnerText.Contains("Fin"))
                {
                    vuelta = node.SelectSingleNode("e:name", manager).InnerText;
                    vueltaPlace = node.SelectSingleNode("e:Point", manager).InnerText;
                }
            }
            return new Recorrido
            {
                Linea = doc.DocumentElement.FirstChild.ChildNodes[0].InnerText,
                CoordenadasIda = idaCoor.Replace("1\n", "").Replace("\n", "").Replace("\t", "").Replace(",0 ", ";").Replace(",0", ""),
                CoordenadasVuelta = vueltaCoord.Replace("1\n", "").Replace("\n", "").Replace("\t", "").Replace(",0 ", ";").Replace(",0", ""),
                PunteroInicio = ida,
                PunteroInicioCoordenada = idaPlace.Replace("1\n", "").Replace("\n", "").Replace("\t", "").Replace(",0 ", ";").Replace(",0", ""),
                PunteroFinal = vuelta,
                PunteroFinalCoordenada = vueltaPlace.Replace("1\n", "").Replace("\n", "").Replace("\t", "").Replace(",0 ", ";").Replace(",0", "")
            };
        }

        public static Recorrido ParseErsaKmzFromXml(XmlDocument doc)
        {
            if (doc == null || doc.DocumentElement == null) return null;
            var manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("e", doc.DocumentElement.NamespaceURI);
            var folder = doc.DocumentElement.FirstChild.SelectSingleNode("e:Folder", manager);
            var placemarks = folder.SelectNodes("e:Placemark", manager);
            string ida = null, vuelta = null, idaCoor = null, vueltaCoord = null, idaPlace = null, vueltaPlace = null;
            foreach (XmlNode node in placemarks)
            {
                if (node.SelectSingleNode("e:name", manager).InnerText.Contains("Ida"))
                    idaCoor = node.SelectSingleNode("e:LineString", manager).InnerText;
                else if (node.SelectSingleNode("e:name", manager).InnerText.Contains("Regreso"))
                    vueltaCoord = node.SelectSingleNode("e:LineString", manager).InnerText;
                else
                {
                    if (node.SelectSingleNode("e:name", manager).InnerText.Contains("(PP)"))
                    {
                        ida = node.SelectSingleNode("e:name", manager).InnerText;
                        idaPlace = node.SelectSingleNode("e:Point", manager).InnerText;
                    }
                    else// if (node.SelectSingleNode("e:name", manager).InnerText.Contains("Fin"))
                    {
                        vuelta = node.SelectSingleNode("e:name", manager).InnerText;
                        vueltaPlace = node.SelectSingleNode("e:Point", manager).InnerText;
                    }
                }
            }
            return new Recorrido
            {
                Linea = doc.DocumentElement.FirstChild.ChildNodes[0].InnerText,
                CoordenadasIda = idaCoor.Replace("1\n", "").Replace("\n", "").Replace("\t", "").Replace(",0 ", ";").Replace(",0", ""),
                CoordenadasVuelta = vueltaCoord.Replace("1\n", "").Replace("\n", "").Replace("\t", "").Replace(",0 ", ";").Replace(",0", ""),
                PunteroInicio = ida,
                PunteroInicioCoordenada = idaPlace.Replace("1\n", "").Replace("\n", "").Replace("\t", "").Replace(",0 ", ";").Replace(",0", ""),
                PunteroFinal = vuelta,
                PunteroFinalCoordenada = vueltaPlace.Replace("1\n", "").Replace("\n", "").Replace("\t", "").Replace(",0 ", ";").Replace(",0", "")
            };
        }
    }

    public class Recorrido2
    {
        public string RecorridosActualesBuscarResult { get; set; }
    }

    public class Recorrido2Data
    {
        public string calles { get; set; }
        public string color { get; set; }
        public string Corredor { get; set; }
        public string corredores_id { get; set; }
        public string Empresas_ID { get; set; }
        public string empresa { get; set; }
        public string esida { get; set; }
        public string linea { get; set; }
        public string lineas_id { get; set; }
        public string recorridos_id { get; set; }
        public string recorridoslinea_id { get; set; }
        public string sentido { get; set; }
        public string sentidoDescri { get; set; }
        public string sentidoida { get; set; }
        public string sentidovuelta { get; set; }
    }

    public class RecorridoDetalleResult
    {
        public string GetByRecorridosIdResult { get; set; }
    }

    public class RecorridoDetalle
    {
        public string esPunto { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Orden { get; set; }
        public string ParadasRecorridos_ID_Cercano { get; set; }
        public string Recorridos_ID { get; set; }
        public string RecorridosDetalle_ID { get; set; }
        public string Vigente { get; set; }
    }
}