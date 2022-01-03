using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AproturWeb.Models
{
    public class MateriaDocumentoViewModel
    {
        public int DocumentoId { get; set; }

        public IEnumerable<SelectListItem> Materias { get; set; }

        [Display(Name = "MateriaId")]
        public int MateriaId { get; set; }
    }
}
