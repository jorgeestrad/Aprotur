
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("TipoFuenteBibliografica", Schema = "Proyectos")]
    public class TipoFuenteBibliografica
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tipo de Fuente Bibilográfica")]
        [Required(ErrorMessage = "Debe ingresar el nombre del Tipo de de Fuente Bibilográfica!")]
        [MaxLength(200, ErrorMessage = "El Nombre de Fuente Bibilográfica puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        public ICollection<Documento> Documentos { get; set; }
    }
}
