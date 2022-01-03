using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AproturWeb.Data.Entities
{

    [Table("PermisosPorUsuario", Schema = "Seguridad")]
    public class PermisosPorUsuario
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "UserRequired")]
        public string UserId { get; set; }

        public User User { get; set; }

        [Display(Name = "Objecto")]
        [Required(ErrorMessage = "ObjectRequired")]
        public int ObjetoId { get; set; }

        public Objeto Objeto { get; set; }

        [MaxLength(255)]
        [Display(Name = "Read")]
        [Required(ErrorMessage = "ReadRequired")]
        public string Read { get; set; }

        [MaxLength(255)]
        [Display(Name = "Insert")]
        [Required(ErrorMessage = "InsertRequired")]
        public string Insert { get; set; }

        [MaxLength(255)]
        [Display(Name = "Update")]
        [Required(ErrorMessage = "UpdateRequired")]
        public string Update { get; set; }

        [MaxLength(255)]
        [Display(Name = "Delete")]
        [Required(ErrorMessage = "DeleteRequired")]
        public string Delete { get; set; }

        [MaxLength(255)]
        [Display(Name = "Download")]
        [Required(ErrorMessage = "DownloadRequired")]
        public string Download { get; set; }

        [MaxLength(255)]
        [Required]
        public string Hash { get; set; }


    }
}
