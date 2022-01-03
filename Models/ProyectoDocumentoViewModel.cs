using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPlus.Models
{
    public class ProyectoDocumentoViewModel
    {
        public int DocumentoId { get; set; }

        public IEnumerable<SelectListItem> Proyectos { get; set; }

        [Display(Name = "ProyectoId")]
        public int ProyectoId { get; set; }
    }
}
