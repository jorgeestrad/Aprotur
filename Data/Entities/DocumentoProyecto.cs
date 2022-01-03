using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GeoPlus.Data.Entities
{
    [Table("DocumentoProyecto", Schema = "Proyectos")]
    public class DocumentoProyecto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Documento")]
        public int DocumentoId { get; set; }

        public Documento Documento { get; set; }

        [Required]
        [Display(Name = "Proyecto")]
        public int ProyectoId { get; set; }

        public Proyecto Proyecto { get; set; }
    }
}
