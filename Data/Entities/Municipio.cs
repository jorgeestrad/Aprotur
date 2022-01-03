using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoPlus.Data.Entities
{
    [Table("Municipio", Schema = "General")]
    public class Municipio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Departamento")]
        public int DepartamentoId { get; set; }

        public Departamento Departamento { get; set; }

        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "Debe ingresar el nombre del municipio!")]
        [MaxLength(50, ErrorMessage = "El Nombre del Municipio puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        [Display(Name = "Código DANE")]
        [Required(ErrorMessage = "Debe ingresar el código del DANE asociado al municipio!")]
        [MaxLength(5, ErrorMessage = "El Código DANE puede tener hasta {1} caracteres!")]
        public string CodigoDane { get; set; }

        public ICollection<KMLMunicipio> KMLMunicipios { get; set; }

        public ICollection<CoberturaDocumento> CoberturaDocumentos { get; set; }


    }
}
