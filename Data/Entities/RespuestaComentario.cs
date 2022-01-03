using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GeoPlus.Data.Entities
{
    [Table("RespuestaComentario", Schema = "Proyectos")]
    public class RespuestaComentario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Comentario")]
        public int ComentarioId { get; set; }

        public Comentario Comentario { get; set; }

        [Required]
        [Display(Name = "Respuesta")]
        public string Respuesta { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
    }
}
