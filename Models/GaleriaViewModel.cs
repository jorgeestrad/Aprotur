using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace AproturWeb.Models
{
    public class GaleriaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar el nombre de la foto!")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debe ingresar la descripción de la foto!")]
        public string Descripcion { get; set; }

        [Required]
        public int ProyectoId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar la foto!")]
        public IFormFile Foto { get; set; }

        public string RutaFoto { get; set; }


    }
}
