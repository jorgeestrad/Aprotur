using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AproturWeb.Data.Entities
{
    public class Rol : IdentityRole
    {

        [Display(Name = "Descripción del Rol")]
        [Required(ErrorMessage = "Descripcion del rol es requiredo")]
        [MaxLength(400, ErrorMessage = "La Descripción no puede tener más de 400 caracteres")]
        public string Descripcion { get; set; }
               

    }
}
