using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("Subregion", Schema = "General")]
    public class Subregion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Departamento")]
        public int DepartamentoId { get; set; }

        public Departamento Departamento { get; set; }

        [Display(Name = "Subregión")]
        [Required(ErrorMessage = "Debe ingresar el nombre de la subregión")]
        [MaxLength(50, ErrorMessage = "El Nombre de la subregión puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }
    }
}
