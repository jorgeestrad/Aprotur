namespace AproturWeb.Models
{
    using AproturWeb.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ResultadoBusquedaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar el Tipo de Documento!")]
        [Display(Name = "Tipo de Documento")]
        public int TipoDocumentoId { get; set; }

        public TipoDocumento TipoDocumento { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar el Tipo de Fuente Bibliográfica!")]
        [Display(Name = "Tipo de Fuente Bibliográfica")]
        public int TipoFuenteBibliograficaId { get; set; }

        public TipoFuenteBibliografica TipoFuenteBibliografica { get; set; }

        [Display(Name = "Formato del Documento")]
        public int FormatoDocumentoId { get; set; }

        public FormatoDocumento FormatoDocumento { get; set; }

        [Display(Name = "Año")]
        public int Anio { get; set; }

        [Display(Name = "País")]
        public int PaisId { get; set; }

        public Pais Pais { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Proyecto")]
        public string Proyecto { get; set; }

        [Display(Name = "Tema Central")]
        public string TemaCentral { get; set; }

        [Display(Name = "Aporte al Documento")]
        public string AporteDocumento { get; set; }

        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Resultado y Conclusiones")]
        public string Resultado { get; set; }

        [Display(Name = "Referencia APA")]
        public string ReferenciaAPA { get; set; }

        [Display(Name = "Documento")]
        public byte[] Archivo { get; set; }

        [Display(Name = "Ruta (url) del documento")]
         public string Ruta { get; set; }

        [Display(Name = "Enlace de Acceso")]
        public string Enlace { get; set; }

        [Display(Name = "Autor")]
        public string Autor { get; set; }

        [Display(Name = "Fecha de Publicación")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public DateTime FechaPublicacion { get; set; }

        [Display(Name = "Resumen del documento")]
        public string Resumen { get; set; }
    }
}
