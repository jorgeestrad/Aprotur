using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AproturWeb.Data.Entities
{
    [Table("TipoObjeto", Schema = "Seguridad")]
    public class TipoObjeto
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tipo de Objeto")]
        [Required(ErrorMessage = "Debe ingresar el nombre del Tipo de Objeto!")]
        [MaxLength(50, ErrorMessage = "El Nombre del Tipo de Objeto puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        public ICollection<Objeto> Objetos { get; set; }

        [DisplayName("Número de Objetos")]
        public int NumeroDepartamentos => Objetos == null ? 0 : Objetos.Count;
    }
}
