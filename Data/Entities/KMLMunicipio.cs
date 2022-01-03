using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("KMLMunicipio", Schema = "Proyectos")]
    public class KMLMunicipio
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage ="Se debe seleccionar el Municipio!")]
        [Display(Name = "Municipio")]
        public int MunicipioId { get; set; }

        public Municipio Municipio { get; set; }

        [Display(Name = "Nombre del KML")]
        [Required(ErrorMessage = "Debe ingresar el nombre del KML asociado al municipio!")]
        [MaxLength(200, ErrorMessage = "El Nombre del KML puede tener hasta {1} caracteres!")]
        public byte[] KML { get; set; }

        [MaxLength(1000, ErrorMessage = "La ruta del KML puede tener hasta {1} caracteres!")]
        [Required (ErrorMessage = "Debe ingresar la Ruta del KML")]
        public string RutaKML { get; set; }
    }
}
