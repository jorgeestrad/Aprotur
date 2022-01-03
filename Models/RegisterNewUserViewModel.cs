namespace AproturWeb.Models
{
    using AproturWeb.Data.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RegisterNewUserViewModel
    {
        [Required(ErrorMessage = "Los Nombres del Usuario son requeridos!")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los Apellidos del Usuario son requeridos!")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Display(Name = "Tipo de documento")]
        public TipoDocumento TipoDocumento { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        public string Documento { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        public string Address { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{Nombres} {Apellidos}";

        [Display(Name = "Tipo de usuario")]
        public IdentityRole UserType { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El Correo Electrónico es requerido!")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La contraseña es requerida!")]
        [MinLength(6)]
        public string Password { get; set; }

        [Display(Name = "Confirmar la Contraseña")]
        [Required(ErrorMessage = "La confirmación de la contraseña es requerida!")]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
