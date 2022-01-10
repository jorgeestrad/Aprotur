using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AproturWeb.Models
{
    public class PaisViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el nombre del País!")]
        [MaxLength(50, ErrorMessage = "El Nombre del País puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }
    }
}
