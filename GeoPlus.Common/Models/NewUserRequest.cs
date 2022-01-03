namespace Aprotur.Common.Models
{
    using Aprotur.Common.Enums;
    using System.ComponentModel.DataAnnotations;
    public  class NewUserRequest
    {
        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombres { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellidos { get; set; }

        [Display(Name = "Tipo de documento")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int TipoDocumentoId { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Documento { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} carácteres.")]
        public string Address { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{Nombres} {Apellidos}";

        [Display(Name = "Tipo de usuario")]
        public UserType UserType { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
