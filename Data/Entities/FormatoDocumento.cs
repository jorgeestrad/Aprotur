using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AproturWeb.Data.Entities
{
    [Table("FormatoDocumento", Schema = "Proyectos")]
    public class FormatoDocumento
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Formato delDocumento")]
        [Required(ErrorMessage = "Debe ingresar el nombre del Formato Documento!")]
        [MaxLength(50, ErrorMessage = "El Nombre del Formato del Documento puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        [Display(Name = "Extensión")]
        [Required(ErrorMessage = "Debe ingresar la extensión del archivo para el Formato del Documento!")]
        [MaxLength(5, ErrorMessage = "El Nombre del la extensión del Formato del Documento puede tener hasta {1} caracteres!")]
        public string Extension { get; set; }

        public int Id_Local { get; set; }

        public ICollection<Documento> Documentos { get; set; }
    }
}
