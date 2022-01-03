using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("Materia", Schema = "General")]
    public class Materia
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el nombre de la Materia!")]
        [MaxLength(50, ErrorMessage = "El Nombre de la Materia puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        public ICollection<MateriaDocumento> MateriaDocumentos { get; set; }

    }
}
