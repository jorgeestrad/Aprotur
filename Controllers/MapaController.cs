using GeoPlus.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPlus.Controllers
{
    public class MapaController : Controller
    {
        private readonly DataContext _context;

        public MapaController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetProyecto(string id)
        {
            var p = JsonSerializer.Deserialize<string[]>(id.Substring(1,id.Length-2));
            var proyecto = this._context.Proyectos.Where(f => f.Nombre == p[1]).FirstOrDefault();
            return this.Json(proyecto);
        }
    }
}
