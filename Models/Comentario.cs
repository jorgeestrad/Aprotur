namespace AproturWeb.Models
{
    using AproturWeb.Data.Entities;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class ComentarioEnviado
    {
        [JsonProperty(PropertyName = "idComentario")]
        public int IdComentario { get; set; }
        
        [JsonProperty(PropertyName = "proyectoId")]
        public int ProyectoId { get; set; }

        [JsonProperty(PropertyName = "fecha")]
        public DateTime Fecha { get; set; }

        [JsonProperty(PropertyName = "comentarioDado")]
        public string ComentarioDado { get; set; }

        public ICollection<RespuestaComentario> RespuestasEnviadas { get; set; }
    }
}
