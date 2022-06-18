using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{
    [Table("Proyecto", Schema = "Proyectos")]
    public class Proyecto
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Proyecto")]
        [Required(ErrorMessage = "Debe ingresar el nombre del proyecto!")]
        [MaxLength(200, ErrorMessage = "El Nombre del Proyecto puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }


        [Display(Name = "KML_KMZ")]
        public byte[] KML { get; set; }

        [MaxLength(1000, ErrorMessage = "La ruta del KML puede tener hasta {1} caracteres!")]
        [Required(ErrorMessage = "Debe ingresar la Ruta del KML")]
        public string RutaKML { get; set; }

        [Display(Name = "Longitud")]
        public double? Longitud { get; set; }

        [Display(Name = "Latitud")]
        public double? Latitud { get; set; }
       
        public int TipoGeograficoId { get; set; }

        public byte[] Imagen { get; set; }

        public byte[] Icono { get; set; }

        public ICollection<DocumentoProyecto> DocumentosProyectos { get; set; }

        public ICollection<Comentario> Comentarios { get; set; }

        public ICollection<GaleriaImagenesProyecto> GaleriaImagenesProyectos { get; set; }

    }
}
