namespace AproturWeb.Models
{
    using Newtonsoft.Json;
    using System;

    public class RespuestaEnvianda
    {
        [JsonProperty(PropertyName = "idRespuesta")]
        public int IdRespuesta { get; set; }

        [JsonProperty(PropertyName = "comentarioId")]
        public int ComentarioId { get; set; }

        [JsonProperty(PropertyName = "fecha")]
        public DateTime Fecha { get; set; }

        [JsonProperty(PropertyName = "respuestaDada")]
        public string RespuestaDada { get; set; }
    }
}
