using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("TipoDocumento", Schema = "Proyectos")]
    public class TipoDocumento
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tipo de Documento")]
        [Required(ErrorMessage = "Debe ingresar el nombre del Tipo de Documento!")]
        [MaxLength(200, ErrorMessage = "El Nombre del Tipo de Documento puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        public ICollection<Documento> Documentos { get; set; }
    }
}
