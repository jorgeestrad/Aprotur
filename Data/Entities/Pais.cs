using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoPlus.Data.Entities
{
    [Table("Pais", Schema = "General")]
    public class Pais
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "País")]
        [Required (ErrorMessage = "Debe ingresar el nombre del país!")]
        [MaxLength(50, ErrorMessage = "El Nombre del País puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        public ICollection<Departamento> Departamentos { get; set; }

        [DisplayName("Número de Departamentos")]
        public int NumeroDepartamentos => Departamentos == null ? 0 : Departamentos.Count;
    }
}
