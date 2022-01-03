using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("MateriaDocumento", Schema = "Proyectos")]
    public class MateriaDocumento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Documento")]
        public int DocumentoId { get; set; }

        public Documento Documento { get; set; }

        [Required]
        [Display(Name = "Materia")]
        public int MateriaId { get; set; }

        public Materia Materia { get; set; }

    }
}
