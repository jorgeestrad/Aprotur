using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace AproturWeb.Models
{
    public class TipoFuenteBibliograficaViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el nombre del Tipo de Fuente Bibliográfica!")]
        [MaxLength(50, ErrorMessage = "El Nombre del Tipo de Fuente Bibliográfica puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }
    }
}
