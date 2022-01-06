using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AproturWeb.Models
{
    public class ProyectoViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el nombre del proyecto!")]
        [MaxLength(200, ErrorMessage = "El Nombre del Proyecto puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }


        [Display(Name = "KML_KMZ")]
        public IFormFile KML { get; set; }

        [MaxLength(1000, ErrorMessage = "La ruta del KML puede tener hasta {1} caracteres!")]
        public string RutaKML { get; set; }

        [Required(ErrorMessage = "Se debe ingresar la Longitud de las coordenadas del proyecto")]
        public double Longitud { get; set; }

        [Required(ErrorMessage = "Se debe ingresar la Latitud de las coordenadas del proyecto")]
        public double Latitud { get; set; }

        public int TipoGeograficoId { get; set; }

      
    }
}
