namespace AproturWeb.Models
{
    using AproturWeb.Data.Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class BusquedaViewModel
    {
        [Display(Name = "Nombre del Proyecto")]
        public string Proyecto { get; set; }
        [Display(Name = "Título del Documento")]
        public string TituloDocumento { get; set;}
        [Display(Name = "Tipo de Documento")]
        public int TipoDocumentoId { get; set; }
        [Display(Name = "Formato del Documento")]
        public int FormatoDocumentoId { get; set; }
        [Display(Name = "País")]
        public int PaisId { get; set; }
        [Display(Name = "Tema Central")]
        public string TemaCentral { get; set; }
        [Display(Name = "Autor")]
        public string Autor { get; set; }
        [Display(Name = "Descriptor")]
        public int MateriaId { get; set; }

        /// <summary>
        /// Tipos de Documentos
        /// </summary>
        public IEnumerable<SelectListItem> TiposDocumentos { get; set; }
        /// <summary>
        /// Tipos de Documentos
        /// </summary>
        public IEnumerable<SelectListItem> FormatosDocumentos { get; set; }
        /// <summary>
        /// Tipos de Documentos
        /// </summary>
        public IEnumerable<SelectListItem> Paises { get; set; }

        /// <summary>
        /// Materias
        /// </summary>
        public IEnumerable<SelectListItem> Materias { get; set; }

        /// <summary>
        /// Documentos
        /// </summary>
        public List<Documento> Documentos { get; set; }

    }
}
