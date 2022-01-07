﻿using System.Diagnostics;
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

        public IActionResult Search()
        {
            try
            {
                    BusquedaViewModel viewModel = new BusquedaViewModel
                    {
                        TiposDocumentos = GetTiposDocumentos(),
                        Paises = GetPaises(),
                        FormatosDocumentos = GetFormatosDocumentos(),
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
                            busquedaVM.TipoDocumentoId == 0 &&
                            busquedaVM.TituloDocumento == null &&
                            busquedaVM.TemaCentral == null)
                        {
                            BusquedaViewModel viewModel = new BusquedaViewModel
                            {
                                TiposDocumentos = GetTiposDocumentos(),
                                Paises = GetPaises(),
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

        public IActionResult ResultSearch(BusquedaViewModel busquedaVM)
        {
            try
            {
                if (busquedaVM.Proyecto == null &&
                             busquedaVM.Autor == null &&
                             busquedaVM.PaisId == 0 &&
                             busquedaVM.TipoDocumentoId == 0 &&
                             busquedaVM.TituloDocumento == null &&
                             busquedaVM.TemaCentral == null)
                {
                    BusquedaViewModel viewModel = new BusquedaViewModel
                    {
                        TiposDocumentos = GetTiposDocumentos(),
                        Paises = GetPaises(),
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
                    Select(s => new AproturWeb.Data.Entities.Documento
                    {
                        Id = s.Id,
                        Titulo = s.Titulo,
                        Anio = s.Anio,
                        Autor = s.Autor,
                        FormatoDocumento = s.FormatoDocumento,
                    }).ToList();
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
                string filePath = $"http://190.147.168.232/Aprotur/kmz/{rutaKml.RutaKML}";
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
                        Resultado = s.Documento.Resultado,
                        Resumen = s.Documento.Resumen,
                        AporteDocumento = s.Documento.AporteDocumento,
                        TemaCentral = s.Documento.TemaCentral,
                        ReferenciaAPA = s.Documento.ReferenciaAPA,
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
