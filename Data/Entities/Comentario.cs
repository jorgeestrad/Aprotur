using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoPlus.Data.Entities
{
    [Table("Comentario", Schema = "Proyectos")]
    public class Comentario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Proyecto")]
        public int ProyectoId { get; set; }

        public Proyecto Proyecto { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        public ICollection<RespuestaComentario> RespuestasComentario { get; set; }

    }
}
