using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace AproturWeb.Models
{
    public class TipoDocumentoViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el nombre del Tipo de Documento!")]
        [MaxLength(50, ErrorMessage = "El Nombre del Tipo de Documento puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }
    }
}
