using System.Collections.Generic;

namespace QueNoSePaseWebService.Models
{
    public class Frecuencias
    {
        public string GetFrecuenciaByLineasDiasResult{ get; set; }
    }

    public class FrecuenciaLineaDias
    {
        public string Corredores_ID { get; set; }
        public string Dias_Id { get; set; }
        public string Frecuencia { get; set; }
        public string Frecuencias_ID { get; set; }
        public string HoraDesde { get; set; }
        public string HoraHasta { get; set; }
        public string HorarioDescripcion { get; set; }
        public string HorDescri { get; set; }
        public string Horarios_ID { get; set; }
        public string Lineas_ID { get; set; }
        public string NombreLinea { get; set; }
        public string PuntosDestacados { get; set; }
        public string SentidoIda { get; set; }
        public string SentidoVuelta { get; set; }
        public string tipoHorarios_Id { get; set; }
        public string VelocidadComercial { get; set; }
        public string Vigente { get; set; }
    }
}