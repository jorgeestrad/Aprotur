using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("GaleriaImagenesProyecto", Schema = "Proyectos")]
    public class GaleriaImagenesProyecto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Proyecto")]
        public int ProyectoId { get; set; }

        public Proyecto Proyecto { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        [MaxLength(70)]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Foto")]
        public byte[] Foto { get; set; }
    }
}
