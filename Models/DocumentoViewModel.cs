using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AproturWeb.Models
{
    public class DocumentoViewModel
    {
         public int Id { get; set; }


        [Required(ErrorMessage = "Se debe seleccionar el Tipo de Documento!")]
        [Display(Name = "Tipo de Documento")]
        public int TipoDocumentoId { get; set; }


        [Required(ErrorMessage = "Se debe seleccionar el Tipo de Fuente Bibliográfica!")]
        [Display(Name = "Tipo de Fuente Bibliográfica")]
        public int TipoFuenteBibliograficaId { get; set; }

     
        [Required(ErrorMessage = "Se debe seleccionar el Formato del Documento!")]
        [Display(Name = "Formato del Documento")]
        public int FormatoDocumentoId { get; set; }

      
        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Documento!")]
        [MaxLength(200, ErrorMessage = "El Nombre del Documento puede tener hasta {1} caracteres!")]
        public string Nombre { get; set; }

        [Display(Name = "Tema Central")]
        [MaxLength(4000, ErrorMessage = "El Tema Central del Documento puede tener hasta {1} caracteres!")]
        public string TemaCentral { get; set; }

        [Display(Name = "Aporte al Documento")]
        [MaxLength(4000, ErrorMessage = "El Aporte al Documento puede tener hasta {1} caracteres!")]
        public string AporteDocumento { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Debe ingresar el Título del Documento!")]
        [MaxLength(200, ErrorMessage = "El Título del Documento puede tener hasta {1} caracteres!")]
        public string Titulo { get; set; }

        [Display(Name = "Resultado y Conclusiones")]
        [MaxLength(4000, ErrorMessage = "El Resultado y Conclusiones del Documento puede tener hasta {1} caracteres!")]
        public string Resultado { get; set; }

        [Display(Name = "Referencia APA")]
        [MaxLength(4000, ErrorMessage = "La Referencia APA del Documento puede tener hasta {1} caracteres!")]
        public string ReferenciaAPA { get; set; }

        [Display(Name = "Archivo")]
        public IFormFile Archivo { get; set; }

        [Display(Name = "Ruta (url) del documento")]
        [Required(ErrorMessage = "Debe ingresar la ruta del Documento!")]
        [MaxLength(400, ErrorMessage = "La ruta del Documento puede tener hasta {1} caracteres!")]
        public string Ruta { get; set; }

        [Display(Name = "Enlace de Acceso")]
        [MaxLength(400, ErrorMessage = "El Enlace de acceso puede tener hasta {1} caracteres!")]
        public string Enlace { get; set; }

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
               

        /// <summary>
        /// Tipos de Documentos
        /// </summary>
        public IEnumerable<SelectListItem> TiposDocumentos { get; set; }

        /// <summary>
        /// Tipos de Documentos
        /// </summary>
        public IEnumerable<SelectListItem> TiposFuentesBibliograficas { get; set; }

        /// <summary>
        /// Tipos de Documentos
        /// </summary>
        public IEnumerable<SelectListItem> FormatosDocumentos { get; set; }

    }
}
