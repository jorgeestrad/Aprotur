using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AproturWeb.Models;
using AproturWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Threading.Tasks;
using AproturWeb.Helpers;

namespace GeoPlus.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IUserHelper userHelper;
        private readonly IWebHostEnvironment hostingEnvironment;
        public HomeController(ILogger<HomeController> logger, IUserHelper userHelper, DataContext context, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
            this.userHelper = userHelper;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.Name != null)
            {
                var user = userHelper.GetUserByEmail(this.User.Identity.Name);

                if (!user.EmailConfirmed) return RedirectToAction("UserNotEnabled", "Home");
               
            }

            return View(_context.Proyectos.ToList());
        }

        public IActionResult UserNotEnabled()
        {
            return View();
        }

        [HttpGet("GetKmlProyecto")]
        public IActionResult GetKmlProyecto(string nombre)
        {
            try
            {
                var rutaKml = _context.Proyectos
                    .Include(i => i.DocumentosProyectos)
                    .Where(f => f.Nombre == nombre).FirstOrDefault();

                string uploadsFolder = hostingEnvironment.ContentRootPath;
                string filePath = $"http://190.147.168.232/GeoPlus/kmz/{rutaKml.RutaKML}";
                rutaKml.RutaKML = filePath;
                return Ok(rutaKml);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }

        [HttpGet("GetDocumentosProyecto")]
        public IActionResult GetDocumentosProyecto(int id)
        {
            try
            {
                var documentos = _context.DocumentoProyectos
                    .Include(i => i.Documento)
                    .Where(f => f.ProyectoId == id)
                    .Select(s => new DocumentoViewModel
                    {
                        Id = s.Documento.Id,
                        Autor = s.Documento.Autor,
                        Descripcion = s.Documento.Descripcion,
                        FechaPublicacion = s.Documento.FechaPublicacion,
                        Nombre = s.Documento.Nombre,
                        Ruta = s.Documento.Ruta,
                        TipoDocumentoId = s.Documento.TipoDocumentoId,
                        Titulo = s.Documento.Titulo,
                    });
                return Ok(documentos);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }
    

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
      
    }
}
