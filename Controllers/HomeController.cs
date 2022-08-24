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
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using AproturWeb.Data.Entities;

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

        public IActionResult Index(int? id)
        {
            try
            {
                if (this.User.Identity.Name != null)
                {
                    var user = userHelper.GetUserByEmail(this.User.Identity.Name);

                    if (!user.EmailConfirmed) return RedirectToAction("UserNotEnabled", "Home");

                }

                if (id != null)
                {
                    var rutaKml = _context.Proyectos
                        .Include(i => i.DocumentosProyectos)
                        .Where(f => f.Id == id).FirstOrDefault();

                    string uploadsFolder = hostingEnvironment.ContentRootPath;
                    string filePath = $"http://3.19.150.196/Aprotur/kmz/{rutaKml.RutaKML}";
                    rutaKml.RutaKML = filePath;
                    return View(rutaKml);
                }
                else
                    return View(new Proyecto { Id = 0});
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }

        public IActionResult AreasProtegidas()
        {
            if (this.User.Identity.Name != null)
            {
                var user = userHelper.GetUserByEmail(this.User.Identity.Name);

                if (!user.EmailConfirmed) return RedirectToAction("UserNotEnabled", "Home");

            }

            return View(_context.Proyectos.ToList());
        }

      
        public IActionResult Search()
        {
            try
            {
                BusquedaViewModel viewModel = new BusquedaViewModel
                {
                    TiposDocumentos = GetTiposDocumentos(),
                    Paises = GetPaises(),
                    FormatosDocumentos = GetFormatosDocumentos(),
                    Materias = GetMaterias(),
                    Proyectos = GetProyectos(),
                    Autor = "",
                    Proyecto = "",
                    TemaCentral = "",
                    TituloDocumento = ""
                    };
                    return View(viewModel);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }

        private IEnumerable<SelectListItem> GetProyectos()
        {
            try
            {
                var la = from s in this._context.Proyectos select s;

                var list = la
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "Seleccione el Proyecto",
                    Value = "0"
                });

                return list;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public IActionResult Bioblitz()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="busquedaVM"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(BusquedaViewModel busquedaVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (busquedaVM.Proyecto == null &&
                            busquedaVM.Autor == null &&
                            busquedaVM.PaisId == 0 &&
                            busquedaVM.ProyectoId == 0 &&
                            busquedaVM.MateriaId == 0 &&
                            busquedaVM.TipoDocumentoId == 0 &&
                            busquedaVM.FormatoDocumentoId == 0 &&
                            busquedaVM.TituloDocumento == null &&
                            busquedaVM.TemaCentral == null)
                        {
                            BusquedaViewModel viewModel = new BusquedaViewModel
                            {
                                TiposDocumentos = GetTiposDocumentos(),
                                Paises = GetPaises(),
                                FormatosDocumentos = GetFormatosDocumentos(),
                                Materias = GetMaterias(),
                                Proyectos = GetProyectos(),
                                Autor = "",
                                Proyecto = "",
                                TemaCentral = "",
                                TituloDocumento = ""
                            };
                            return View(viewModel);
                        }
                        else
                        {
                            return RedirectToAction(nameof(ResultSearch), busquedaVM);
                        } 
                    }
                    catch (Exception exp)
                    {
                        return NotFound(exp.Message);
                    }
                }
                return View(busquedaVM);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }

        private IEnumerable<SelectListItem> GetMaterias()
        {
            try
            {
                var la = from s in this._context.Materias select s;

                var list = la
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "Seleccione el Descriptor",
                    Value = "0"
                });

                return list;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public IActionResult ResultSearch(BusquedaViewModel busquedaVM)
        {
            try
            {
                if (busquedaVM.Proyecto == null &&
                             busquedaVM.Autor == null &&
                             busquedaVM.PaisId == 0 &&
                             busquedaVM.ProyectoId == 0 &&
                             busquedaVM.MateriaId == 0 && 
                             busquedaVM.TipoDocumentoId == 0 &&
                             busquedaVM.FormatoDocumentoId == 0 &&
                             busquedaVM.TituloDocumento == null &&
                             busquedaVM.TemaCentral == null)
                {
                    BusquedaViewModel viewModel = new BusquedaViewModel
                    {
                        TiposDocumentos = GetTiposDocumentos(),
                        Paises = GetPaises(),
                        Materias = GetMaterias(),
                        Proyectos = GetProyectos(),
                        FormatosDocumentos = GetFormatosDocumentos(),
                        Autor = "",
                        Proyecto = "",
                        TemaCentral = "",
                        TituloDocumento = ""
                    };
                    return View(viewModel);
                }
                else
                {
                    busquedaVM.Documentos = GetDocumentosBusqueda(busquedaVM);
                    return View(busquedaVM);
                }
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }

        private List<AproturWeb.Data.Entities.Documento> GetDocumentosBusqueda(BusquedaViewModel busquedaVM)
        {
            try
            {
                List<Documento> docuMat = new List<Documento>();
                List<Documento> docuProy = new List<Documento>();

               

                if (busquedaVM.MateriaId > 0)
                {
                   

                         docuMat = this._context.MateriaDocumentos
                           .Include(i => i.Documento)
                           .Where(f => f.MateriaId == busquedaVM.MateriaId)
                           .Select(s => s.Documento)
                           .Select(s => new AproturWeb.Data.Entities.Documento
                           {
                               Id = s.Id,
                               Nombre = s.Nombre,
                               Titulo = s.Titulo,
                               Ruta = s.Ruta,
                               FormatoDocumentoId = s.FormatoDocumentoId,
                               Anio = s.Anio,
                               Autor = s.Autor,
                               Enlace = s.Enlace,
                               FormatoDocumento = s.FormatoDocumento,
                               MateriaDocumentos = s.MateriaDocumentos,
                           }).ToList();

                       
                    

                }

                if (busquedaVM.ProyectoId > 0)
                {
                   
                         docuProy = this._context.DocumentoProyectos
                           .Include(i => i.Documento)
                           .Where(f => f.ProyectoId == busquedaVM.ProyectoId)
                           .Select(s => s.Documento)
                           .Select(s => new AproturWeb.Data.Entities.Documento
                           {
                               Id = s.Id,
                               Nombre = s.Nombre,
                               Titulo = s.Titulo,
                               Ruta = s.Ruta,
                               FormatoDocumentoId = s.FormatoDocumentoId,
                               Anio = s.Anio,
                               Autor = s.Autor,
                               Enlace = s.Enlace,
                               FormatoDocumento = s.FormatoDocumento,
                               MateriaDocumentos = s.MateriaDocumentos,
                           }).ToList();

                      
                    

                }

                if (string.IsNullOrEmpty(busquedaVM.TituloDocumento)) busquedaVM.TituloDocumento = "#$#$#$#$#$#$";
                if (string.IsNullOrEmpty(busquedaVM.TemaCentral)) busquedaVM.TemaCentral = "#$#$#$#$#$#$";
                if (string.IsNullOrEmpty(busquedaVM.Autor)) busquedaVM.Autor = "#$#$#$#$#$#$";
                var documentos = this._context.Documentos.
                  Where(f => f.Titulo.ToLower().Trim().Contains(busquedaVM.TituloDocumento.ToLower().Trim()) ||
                          f.TemaCentral.ToLower().Trim().Contains(busquedaVM.TemaCentral.ToLower().Trim()) ||
                          f.Autor.ToLower().Trim().Contains(busquedaVM.Autor.ToLower().Trim()) ||
                          f.PaisId == busquedaVM.PaisId ||
                          f.FormatoDocumentoId == busquedaVM.FormatoDocumentoId ||
                          f.TipoDocumentoId == busquedaVM.TipoDocumentoId
                          ).
                    Include(i => i.FormatoDocumento).
                    Include(i => i.DocumentosProyectos).
                    Include(i => i.MateriaDocumentos).
                    Select(s => new AproturWeb.Data.Entities.Documento
                    {
                        Id = s.Id,
                        Nombre = s.Nombre,
                        Titulo = s.Titulo,
                        Ruta = s.Ruta,
                        FormatoDocumentoId = s.FormatoDocumentoId,
                        Anio = s.Anio,
                        Autor = s.Autor,
                        Enlace = s.Enlace,
                        FormatoDocumento = s.FormatoDocumento,
                        MateriaDocumentos = s.MateriaDocumentos,
                        DocumentosProyectos = s.DocumentosProyectos,
                    }).ToList();

                if (docuMat != null && docuMat.Count > 0)  documentos.AddRange(docuMat);
                if (docuProy != null && docuProy.Count > 0) documentos.AddRange(docuProy);
                return documentos;
            }
            catch (Exception exp)
            {
               throw new Exception(exp.Message);
            }
        }

        private IEnumerable<SelectListItem> GetFormatosDocumentos()
        {
            try
            {
                var la = from s in this._context.FormatosDocumentos select s;

                var list = la
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "Seleccione Formato del Documento",
                    Value = "0"
                });

                return list;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private IEnumerable<SelectListItem> GetPaises()
        {
            try
            {
                var la = from s in this._context.Paises select s;

                var list = la
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "Seleccione el País",
                    Value = "0"
                });

                return list;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Retorna el listado de Tipos de Documentos
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SelectListItem> GetTiposDocumentos()
        {
            try
            {
                var la = from s in this._context.TiposDocumento select s;

                var list = la
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "Seleccione el Tipo de Documento",
                    Value = "0"
                });

                return list;
            }
            catch (Exception exp)
            {
                throw exp;
            }
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
                string filePath = $"http://3.19.150.196/Aprotur/kmz/{rutaKml.RutaKML}";
                rutaKml.RutaKML = filePath;
                return Ok(rutaKml);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }



        public IActionResult GetDocumentosProyectoGen(int? id)
        {
            try
            {
                return View(id);
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
                    .Include(i => i.Documento).ThenInclude(i => i.FormatoDocumento)
                    .Where(f => f.ProyectoId == id)
                    .OrderBy(o => o.Documento.Nombre)
                    .Select(s => new DocumentoViewModel
                    {
                        Id = s.Documento.Id,
                        Autor = s.Documento.Autor,
                        Resultado = s.Documento.Resultado,
                        Resumen = s.Documento.Resumen,
                        AporteDocumento = s.Documento.AporteDocumento,
                        TemaCentral = s.Documento.TemaCentral,
                        ReferenciaAPA = s.Documento.ReferenciaAPA,
                        FechaPublicacion = s.Documento.FechaPublicacion,
                        Nombre = s.Documento.Nombre,
                        Ruta = s.Documento.Ruta,
                        TipoDocumentoId = s.Documento.TipoDocumentoId,
                        FormatoDocumentoId = s.Documento.FormatoDocumento.Id_Local,
                        Titulo = s.Documento.Titulo,
                        Enlace = s.Documento.Enlace,
                      
                    });
                return Ok(documentos);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }


        
        [HttpGet("GetGalleryProyecto")]
        public IActionResult GetGalleryProyecto(int id)
        {
            try
            {
                var fotos = _context.GaleriaImagenesProyectos
                    .Where(f => f.ProyectoId == id)
                    .Select(s => new GaleriaImagenesProyecto
                    {
                        Id = s.Id,
                        Descripcion = s.Descripcion,
                        Nombre = s.Nombre,
                        Foto = s.Foto,
                    });
                return Ok(fotos);
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


        [HttpGet("GetDocumento")]
        public IActionResult GetDocumento(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var documento = _context.Documentos
                    .Select(s => new DocumentoViewModel
                    {
                        Id = s.Id,
                        Titulo = s.Titulo,
                        Ruta = s.Ruta,
                        Portada = s.Portada,
                        Autor = s.Autor,
                        Anio = s.Anio,
                        AporteDocumento = s.AporteDocumento,
                        FormatoDocumentoId = s.FormatoDocumentoId,
                        PaisId = s.PaisId,
                        Resultado = s.Resultado,
                        ReferenciaAPA = s.ReferenciaAPA,
                        TemaCentral = s.TemaCentral,
                        TipoFuenteBibliograficaId = s.TipoFuenteBibliograficaId,
                        TipoDocumentoId = s.TipoDocumentoId,
                        FechaPublicacion = s.FechaPublicacion,
                        FechaPublicacionS = s.FechaPublicacion.ToString("MM/dd/yyyy HH:mm:ss"),
                        Nombre = s.Nombre,
                        Resumen = s.Resumen,
                        Enlace = s.Enlace,
                    })
                    .Where(f => f.Id == id.Value).FirstOrDefault();

                if (documento == null)
                {
                    return NotFound();
                }
                return Ok(documento);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }

    }
}
