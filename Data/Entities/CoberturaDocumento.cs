using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GeoPlus.Data.Entities
{
    [Table("CoberturaDocumento", Schema = "Proyectos")]
    public class CoberturaDocumento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Documento")]
        public int DocumentoId { get; set; }

        public Documento Documento { get; set; }

        [Required]
        [Display(Name = "Municipio")]
        public int MunicipioId { get; set; }

        public Municipio Municipio { get; set; }

    }
}
