using AproturWeb.Data;
using AproturWeb.Data.Entities;
using AproturWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPlus.Controllers
{
    public class DocumentoController : Controller
    {

        private readonly DataContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public DocumentoController(DataContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Consulta el listado de documentos
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                var list = _context.Documentos
                    .Include(i => i.TipoDocumento)
                    .Select(i => new Documento
                    {
                        Id = i.Id,
                        Nombre = i.Nombre,
                        Autor = i.Autor,
                        Anio = i.Anio,
                        ReferenciaAPA = i.ReferenciaAPA,
                        Resumen = i.Resumen,
                        TemaCentral = i.TemaCentral,
                        TipoFuenteBibliograficaId = i.TipoFuenteBibliograficaId,
                        Resultado = i.Resultado,
                        FormatoDocumentoId = i.FormatoDocumentoId,
                        AporteDocumento = i.AporteDocumento,
                        FechaPublicacion = i.FechaPublicacion,
                        Ruta = i.Ruta,
                        TipoDocumento = i.TipoDocumento,
                        TipoDocumentoId = i.TipoDocumentoId,
                        Titulo = i.Titulo,
                    })
                    .ToList();
                return View(list);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }

        /// <summary>
        /// Permite crear un Documento
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            try
            {
                DocumentoViewModel viewModel = new DocumentoViewModel
                {
                    TiposDocumentos = GetTiposDocumentos(),
                    TiposFuentesBibliograficas = GetFuentesBibligraficas(),
                    FormatosDocumentos = GetFormatosDocumentos(),
                    FechaPublicacion = DateTime.Now,
                    Anio = DateTime.Now.Year,
                };
                return View(viewModel);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
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
                    Text = "Seleccione el Formato del Documento",
                    Value = "0"
                });

                return list;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private IEnumerable<SelectListItem> GetFuentesBibligraficas()
        {
            try
            {
                var la = from s in this._context.TiposFuenteBibliograficas select s;

                var list = la
                .Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "Seleccione el Tipo de Fuente Bibliográfica del Documento",
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentoViewModel modelo)
        {
            try
            {
                string uniqueFileName = "";
                if (ModelState.IsValid)
                {
                    try
                    {
                        Documento documento = new Documento
                        {
                            Nombre = modelo.Nombre,
                            Resultado = modelo.Resultado,
                            FechaPublicacion = modelo.FechaPublicacion,
                            TipoDocumentoId = modelo.TipoDocumentoId,
                            Autor = modelo.Autor,
                            Portada = modelo.Portada,
                            Ruta = modelo.Ruta,
                            Titulo = modelo.Titulo,
                            Resumen = modelo.Resumen,
                            AporteDocumento = modelo.AporteDocumento,
                            FormatoDocumentoId = modelo.FormatoDocumentoId,
                            ReferenciaAPA = modelo.ReferenciaAPA,
                            TemaCentral = modelo.TemaCentral,
                            TipoFuenteBibliograficaId = modelo.TipoFuenteBibliograficaId,
                            Anio = modelo.Anio,
                            Enlace = modelo.Enlace,
                        };
                        if (modelo.Archivo != null)
                        {
                            // The image must be uploaded to the images folder in wwwroot
                            // To get the path of the wwwroot folder we are using the inject
                            // HostingEnvironment service provided by ASP.NET Core
                            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "kmz");
                            // To make sure the file name is unique we are appending a new
                            // GUID value and and an underscore to the file name
                            uniqueFileName = Guid.NewGuid().ToString() + "_" + modelo.Archivo.FileName;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                            // Use CopyTo() method provided by IFormFile interface to
                            // copy the file to wwwroot/images folder
                            modelo.Archivo.CopyTo(new FileStream(filePath, FileMode.Create));
                            modelo.Ruta = uniqueFileName;
                            using (var ms = new MemoryStream())
                            {
                                modelo.Archivo.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                documento.Archivo = fileBytes;
                                documento.Ruta = uniqueFileName;
                            }
                        }
                        _context.Add(documento);

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException dbUpdateException)
                    {
                        if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                        {
                            ModelState.AddModelError(string.Empty, "Ya existe este documento.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                        }
                    }
                    catch (Exception exception)
                    {
                        ModelState.AddModelError(string.Empty, exception.Message);
                    }
                }

                modelo.TiposDocumentos = GetTiposDocumentos();
                modelo.TiposFuentesBibliograficas = GetFuentesBibligraficas();
                modelo.FormatosDocumentos = GetFormatosDocumentos();
                return View(modelo);
            }
            catch (Exception exp)
            {
                return NotFound(exp.Message);
            }
        }


        /// <summary>
        /// Permite editar un Documento
        /// </summary>
        /// <param name="id">Identifica el documento</param>
        /// <returns></returns>
        public IActionResult Edit(int? id)
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
                    Resultado = s.Resultado,
                    ReferenciaAPA = s.ReferenciaAPA,
                    TemaCentral = s.TemaCentral,
                    TipoFuenteBibliograficaId = s.TipoFuenteBibliograficaId,
                    TipoDocumentoId = s.TipoDocumentoId,
                    FechaPublicacion = s.FechaPublicacion,
                    Nombre = s.Nombre,
                    Resumen = s.Resumen,
                    Enlace = s.Enlace,
                })
                .Where(f => f.Id == id.Value).FirstOrDefault();
            
            if (documento == null)
            {
                return NotFound();
            }
            documento.TiposDocumentos = GetTiposDocumentos();
            documento.TiposFuentesBibliograficas = GetFuentesBibligraficas();
            documento.FormatosDocumentos = GetFormatosDocumentos();
            return View(documento);
        }

        /// <summary>
        /// Almacena los cambios dados por el usuario
        /// </summary>
        /// <param name="id">Identifica el documento</param>
        /// <param name="proyecto">Objeto de Tipo documento</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocumentoViewModel modelo)
        {
            string uniqueFileName = "";

            if (id != modelo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Documento documento = new Documento
                    {
                        Id = modelo.Id,
                        Nombre = modelo.Nombre,
                        Resultado = modelo.Resultado,
                        FechaPublicacion = modelo.FechaPublicacion,
                        TipoDocumentoId = modelo.TipoDocumentoId,
                        Autor = modelo.Autor,
                        Portada = modelo.Portada,
                        Ruta = modelo.Ruta,
                        Titulo = modelo.Titulo,
                        Resumen = modelo.Resumen,
                        AporteDocumento = modelo.AporteDocumento,
                        FormatoDocumentoId = modelo.FormatoDocumentoId,
                        ReferenciaAPA = modelo.ReferenciaAPA,
                        TemaCentral = modelo.TemaCentral,
                        TipoFuenteBibliograficaId = modelo.TipoFuenteBibliograficaId,
                        Anio = modelo.Anio,
                        Enlace = modelo.Enlace,
                    };

                    if (modelo.Archivo != null)
                    {
                        // The image must be uploaded to the images folder in wwwroot
                        // To get the path of the wwwroot folder we are using the inject
                        // HostingEnvironment service provided by ASP.NET Core
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "kmz");
                        // To make sure the file name is unique we are appending a new
                        // GUID value and and an underscore to the file name
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + modelo.Archivo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        modelo.Archivo.CopyTo(new FileStream(filePath, FileMode.Create));
                        modelo.Ruta = uniqueFileName;
                        using (var ms = new MemoryStream())
                        {
                            modelo.Archivo.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            documento.Archivo = fileBytes;
                            documento.Ruta = uniqueFileName;
                        }
                    }

                    _context.Update(documento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este documento.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            modelo.TiposDocumentos = GetTiposDocumentos();
            modelo.TiposFuentesBibliograficas = GetFuentesBibligraficas();
            modelo.FormatosDocumentos = GetFormatosDocumentos();
            return View(modelo);
        }

        /// <summary>
        /// Permite eliminar un documento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Documento documento = _context.Documentos
                .Select(s => new Documento
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Resultado = s.Resultado,
                    FechaPublicacion = s.FechaPublicacion,
                    TipoDocumentoId = s.TipoDocumentoId,
                    Autor = s.Autor,
                    Portada = s.Portada,
                    Ruta = s.Ruta,
                    Titulo = s.Titulo,
                    Resumen = s.Resumen,
                    AporteDocumento = s.AporteDocumento,
                    FormatoDocumentoId = s.FormatoDocumentoId,
                    ReferenciaAPA = s.ReferenciaAPA,
                    TemaCentral = s.TemaCentral,
                    TipoFuenteBibliograficaId = s.TipoFuenteBibliograficaId,
                    Anio = s.Anio,
                    Enlace = s.Enlace,
                })
                .Where(f => f.Id == id.Value).FirstOrDefault();
            
            if (documento == null)
            {
                return NotFound();
            }
            
            try
            {
                _context.Documentos.Remove(documento);
                await _context.SaveChangesAsync();
                  
            }
            catch 
            {
                return RedirectToAction(nameof(NoDelete));
            }
            return RedirectToAction(nameof(Index));

        }

        #region Proyectos vinculados con el documento

        /// <summary>
        /// Lista los proyectos vinculados con el documento seleccionado
        /// </summary>
        /// <param name="id">Identifica el documento</param>
        /// <returns></returns>
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var documento = this.ObtenerDocumentoConProyectos(id.Value);
            if (documento == null)
            {
                return NotFound();
            }


            return View(documento);
        }

        private Documento ObtenerDocumentoConProyectos(int id)
        {
            try
            {
                var document = this._context.Documentos
                .Where(c => c.Id == id)
                .Select(s => new Documento
                {
                    Nombre = s.Nombre,
                    Resultado = s.Resultado,
                    FechaPublicacion = s.FechaPublicacion,
                    TipoDocumentoId = s.TipoDocumentoId,
                    Autor = s.Autor,
                    Portada = s.Portada,
                    Ruta = s.Ruta,
                    Titulo = s.Titulo,
                    Resumen = s.Resumen,
                    AporteDocumento = s.AporteDocumento,
                    FormatoDocumentoId = s.FormatoDocumentoId,
                    ReferenciaAPA = s.ReferenciaAPA,
                    TemaCentral = s.TemaCentral,
                    TipoFuenteBibliograficaId = s.TipoFuenteBibliograficaId,
                    Anio = s.Anio,
                    Enlace = s.Enlace,
                })
                .FirstOrDefault();

                document.DocumentosProyectos = this._context.DocumentoProyectos.Where(f => f.DocumentoId == id).ToList();
                foreach (DocumentoProyecto documentoProyecto in document.DocumentosProyectos)
                {
                    documentoProyecto.Proyecto = this._context.Proyectos.Select(s => new Proyecto
                    {
                        Id = s.Id,
                        Nombre = s.Nombre,

                    }).Where(f=>f.Id == documentoProyecto.ProyectoId).FirstOrDefault();
                }

                return document;
            }
            catch(Exception exp)
            {
                throw exp;
            }
            
        }

        /// <summary>
        /// Vincula un proyecto al documento seleccionado
        /// </summary>
        /// <param name="id">Identifica el Documento</param>
        /// <returns></returns>
        public IActionResult VincularProyecto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                ProyectoDocumentoViewModel proyectoDocumentoViewModel = new ProyectoDocumentoViewModel
                {
                    DocumentoId = id.Value,
                    Proyectos = this.GetComboProyectos(id.Value),
                };
                return View(proyectoDocumentoViewModel);
            }
            catch (Exception exp)
            {
                return RedirectToAction("ErrorException", "Home", new { Error = exp.InnerException != null ? exp.InnerException.Message : exp.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VincularProyecto(ProyectoDocumentoViewModel view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        this.Vincular(view);
                        return this.RedirectToAction("Details", new { id = view.DocumentoId });
                    }
                    catch (Exception exp)
                    {
                        return NotFound();
                    }
                }
                view.Proyectos = this.GetComboProyectos(view.DocumentoId);
                return View(view);
            }
            catch (Exception exp)
            {
                return NotFound();
            }
        }

        private void Vincular(ProyectoDocumentoViewModel view)
        {
            try
            {
                DocumentoProyecto documentoProyecto = new DocumentoProyecto
                {
                    DocumentoId = view.DocumentoId,
                    ProyectoId = view.ProyectoId,
                };

                this._context.Add(documentoProyecto);
                this._context.SaveChanges();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
      
        /// <summary>
        /// Retorna el listado de proyectos que se pueden vincular al documento
        /// </summary>
        /// <param name="documentoId"></param>
        /// <returns></returns>
        private IEnumerable<SelectListItem> GetComboProyectos(int documentoId)
        {
            try
            {
                var listProyectos = this._context.DocumentoProyectos
                   .Where(f => f.DocumentoId == documentoId).ToList();

                foreach (DocumentoProyecto documentoProyecto in listProyectos)
                {
                    documentoProyecto.Proyecto = this._context.Proyectos.Select(s => new Proyecto
                    {
                        Id = s.Id,
                        Nombre = s.Nombre,
                        RutaKML = s.RutaKML,

                    }).Where(f => f.Id == documentoProyecto.ProyectoId)
                    .FirstOrDefault();
                }

                var listExist = listProyectos.Select(s => s.Proyecto).ToList();

                var _l = this._context.Proyectos.Select(s => new Proyecto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    RutaKML = s.RutaKML,

                }).ToList();

                var _ld = new List<Proyecto>();
                foreach (Proyecto proyecto in _l)
                {
                    bool existe = false;
                    foreach (Proyecto _pro in listExist)
                    {
                        if (proyecto.Id == _pro.Id)
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (!existe) _ld.Add(proyecto);

                }

                var list = _ld.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString(),
                }).ToList();

                list = list.OrderBy(o => o.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = string.Empty,
                    Value = null
                });
                return list;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        
        public async Task<IActionResult> DesvincularProyecto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DocumentoProyecto documentoP = _context.DocumentoProyectos
                     .Where(f => f.Id == id.Value).FirstOrDefault();

            if (documentoP == null)
            {
                return NotFound();
            }

            try
            {
                _context.DocumentoProyectos.Remove(documentoP);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException dbUpdateException)
            {

            }
            return RedirectToAction(nameof(Details), new { id = documentoP.DocumentoId });

        }

        /// <summary>
        /// Lista las Materias vinculadas con el documento seleccionado
        /// </summary>
        /// <param name="id">Identifica el documento</param>
        /// <returns></returns>
        public IActionResult Materias(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var documento = this.ObtenerMateriasConProyectos(id.Value);
            if (documento == null)
            {
                return NotFound();
            }


            return View(documento);
        }

        private object ObtenerMateriasConProyectos(int id)
        {
            try
            {
                var document = this._context.Documentos
                .Where(c => c.Id == id)
                .Select(s => new Documento
                {
                    Autor = s.Autor,
                    ReferenciaAPA = s.ReferenciaAPA,
                    TemaCentral = s.TemaCentral,
                    Anio = s.Anio,
                    AporteDocumento = s.AporteDocumento,
                    Resultado = s.Resultado,
                    Resumen = s.Resumen,
                    FormatoDocumentoId = s.FormatoDocumentoId,
                    TipoFuenteBibliograficaId = s.TipoFuenteBibliograficaId,
                    FechaPublicacion = s.FechaPublicacion,
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Ruta = s.Ruta,
                    TipoDocumentoId = s.TipoDocumentoId,
                    Titulo = s.Titulo,
                    Enlace = s.Enlace,
                })
                .FirstOrDefault();

                document.MateriaDocumentos = this._context.MateriaDocumentos.Where(f => f.DocumentoId == id).ToList();
                foreach (MateriaDocumento materiaP in document.MateriaDocumentos)
                {
                    materiaP.Materia = this._context.Materias.Select(s => new Materia
                    {
                        Id = s.Id,
                        Nombre = s.Nombre,

                    }).Where(f => f.Id == materiaP.MateriaId).FirstOrDefault();
                }

                return document;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        
        /// <summary>
        /// Vincula una Materia al documento seleccionado
        /// </summary>
        /// <param name="id">Identifica el Documento</param>
        /// <returns></returns>
        public IActionResult VincularMateria(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                MateriaDocumentoViewModel proyectoDocumentoViewModel = new MateriaDocumentoViewModel
                {
                    DocumentoId = id.Value,
                    Materias = this.GetComboMaterias(id.Value),
                };
                return View(proyectoDocumentoViewModel);
            }
            catch (Exception exp)
            {
                return RedirectToAction("ErrorException", "Home", new { Error = exp.InnerException != null ? exp.InnerException.Message : exp.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VincularMateria(MateriaDocumentoViewModel view)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        this.VincularMateriaADocumento(view);
                        return this.RedirectToAction("Materias", new { id = view.DocumentoId });
                    }
                    catch (Exception exp)
                    {
                        return NotFound();
                    }
                }
                view.Materias = this.GetComboMaterias(view.DocumentoId);
                return View(view);
            }
            catch (Exception exp)
            {
                return NotFound();
            }
        }

        private void VincularMateriaADocumento(MateriaDocumentoViewModel view)
        {
            try
            {
                MateriaDocumento materiaDocumento = new MateriaDocumento
                {
                    DocumentoId = view.DocumentoId,
                    MateriaId = view.MateriaId,
                };

                this._context.Add(materiaDocumento);
                this._context.SaveChanges();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private IEnumerable<SelectListItem> GetComboMaterias(int documentoId)
        {
            try
            {
                var listMaterias = this._context.MateriaDocumentos
                   .Where(f => f.DocumentoId == documentoId).ToList();

                foreach (MateriaDocumento documentoMateria in listMaterias)
                {
                    documentoMateria.Materia = this._context.Materias.Select(s => new Materia
                    {
                        Id = s.Id,
                        Nombre = s.Nombre,
                      
                    }).Where(f => f.Id == documentoMateria.MateriaId)
                    .FirstOrDefault();
                }

                var listExist = listMaterias.Select(s => s.Materia).ToList();

                var _l = this._context.Materias.Select(s => new Materia
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                }).ToList();

                var _ld = new List<Materia>();
                foreach (Materia materia in _l)
                {
                    bool existe = false;
                    foreach (Materia _pro in listExist)
                    {
                        if (materia.Id == _pro.Id)
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (!existe) _ld.Add(materia);

                }
                var list = _ld.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString(),
                }).ToList();

                list = list.OrderBy(o => o.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = string.Empty,
                    Value = null
                });
                return list;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public async Task<IActionResult> DesvincularMateria(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MateriaDocumento documentoP = _context.MateriaDocumentos
                     .Where(f => f.Id == id.Value).FirstOrDefault();

            if (documentoP == null)
            {
                return NotFound();
            }

            try
            {
                _context.MateriaDocumentos.Remove(documentoP);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException dbUpdateException)
            {

            }
            return RedirectToAction(nameof(Materias), new { id = documentoP.DocumentoId });

        }


        #endregion

        public IActionResult NoDelete()
        {
            return View();
        }
    }
}

