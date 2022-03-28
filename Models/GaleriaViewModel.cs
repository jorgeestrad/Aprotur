using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace AproturWeb.Models
{
    public class GaleriaViewModel
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public int ProyectoId { get; set; }

        public IFormFile Foto { get; set; }

        public string RutaFoto { get; set; }


    }
}
