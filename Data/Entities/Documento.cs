using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeoPlus.Data.Entities
{
    [Table("Documento", Schema = "Proyectos")]
    public class Documento
    {
        [Key]
        public int Id { get; set; }

             
        [Required(ErrorMessage = "Se debe seleccionar el Tipo de Documento!")]
        [Display(Name = "Tipo de Documento")]
        public int TipoDocumentoId { get; set; }

        public TipoDocumento TipoDocumento { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Documento!")]
        [MaxLength(200, ErrorMessage = "El Nombre del Documento puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(4000, ErrorMessage = "La Descripción del Documento puede tener hasta {1} caracteres!")]
        public string Descripcion { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Debe ingresar el Título del Documento!")]
        [MaxLength(200, ErrorMessage = "El Título del Documento puede tener hasta {1} caracteres!")]
        public string Titulo { get; set; }

        [Display(Name = "Documento")]
        public byte[] Archivo { get; set; }

        [Display(Name = "Ruta (url) del documento")]
        [Required(ErrorMessage = "Debe ingresar la ruta del Documento!")]
        [MaxLength(400, ErrorMessage = "La ruta del Documento puede tener hasta {1} caracteres!")]
        public string Ruta { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del autor o creador del Documento!")]
        [MaxLength(100, ErrorMessage = "El Nombre del Autor del Documento puede tener hasta {1} caracteres!")]
        public string Autor { get; set; }

        [Display(Name = "Fecha de Publicación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime FechaPublicacion { get; set; }

        [Display(Name = "Resumen del documento")]
        [MaxLength(4000, ErrorMessage = "El Resumen del Documento puede tener hasta {1} caracteres!")]
        public string Resumen { get; set; }


        [Display(Name = "Portada")]
        public byte[] Portada { get; set; }


        public ICollection<CoberturaDocumento> CoberturaDocumentos { get; set; }

        public ICollection<MateriaDocumento> MateriaDocumentos { get; set; }

        public ICollection<DocumentoProyecto> DocumentosProyectos { get; set; }

        [Display(Name = "NumberoProyectos")]
        public int NumberoProyectos { get { return this.DocumentosProyectos == null ? 0 : this.DocumentosProyectos.Count; } }

    }
}
