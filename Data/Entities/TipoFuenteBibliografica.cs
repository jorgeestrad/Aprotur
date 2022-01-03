
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("TipoFuenteBibliografica", Schema = "Seguridad")]
    public class TipoFuenteBibliografica
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tipo de Documento")]
        [Required(ErrorMessage = "Debe ingresar el nombre del Tipo de Documento!")]
        [MaxLength(50, ErrorMessage = "El Nombre del Tipo de Documento puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        [Display(Name = "Extensión")]
        [Required(ErrorMessage = "Debe ingresar la extensión del archivo para el Tipo de Documento!")]
        [MaxLength(5, ErrorMessage = "El Nombre del la extensión del Tipo de Documento puede tener hasta {1} caracteres!")]
        public string Extension { get; set; }

        public ICollection<Documento> Documentos { get; set; }
    }
}
