using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("Departamento", Schema = "General")]
    public class Departamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "País")]
        public int PaisId { get; set; }

        public Pais Pais { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Debe ingresar el nombre del departamento!")]
        [MaxLength(50, ErrorMessage = "El Nombre del Departamento puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        public ICollection<Subregion> Subregiones { get; set; }

        public ICollection<Municipio> Municipios { get; set; }

        [DisplayName("Número de Subregiones")]
        public int NumeroSubregiones => Subregiones == null ? 0 : Subregiones.Count;

        [DisplayName("Número de Municipios")]
        public int NumeroMunicipioss => Municipios == null ? 0 : Municipios.Count;

    }
}
