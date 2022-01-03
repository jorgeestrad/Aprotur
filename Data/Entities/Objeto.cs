using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoPlus.Data.Entities
{
    [Table("Objeto", Schema = "Seguridad")]
    public class Objeto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TipoObjetoId { get; set; }


        public TipoObjeto TipoObjeto { get; set; }

        [Display(Name = "Nombre del Objeto")]
        [Required(ErrorMessage = "Debe ingresar el nombre del Objeto!")]
        [MaxLength(50, ErrorMessage = "El Nombre del Objeto puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }


        [Display(Name = "Descripción del Objeto")]
        [MaxLength(4000, ErrorMessage = "El Nombre del Objeto puede tener hasta {1} caracteres!")]
        public string Descripcion { get; set; }


        [Required(ErrorMessage = "Debe ingresar la clave primaria del objeto!")]
        public int KeyId { get; set; }

        [Display(Name = "Fecha de creación del objeto")]
        public DateTime? FechaCreacion { get; set; }

        public ICollection<PermisosPorUsuario> PermisosPorUsuarios { get; set; }

        public ICollection<PermisosPorRol>  PermisosPorRoles { get; set; }

    }
}
