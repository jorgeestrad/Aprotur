using AproturWeb.Data;
using AproturWeb.Data.Entities;
using AproturWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPlus.Controllers
{
    /// <summary>
    /// Controlador Proyectos
    /// </summary>
    public class ProyectoController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ProyectoController(DataContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Consulta el listado de proyectos
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(_context.Proyectos.Select(s => new Proyecto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                RutaKML = s.RutaKML,
                TipoGeograficoId = s.TipoGeograficoId
            }).ToList());
        }

        /// <summary>
        /// Permite crear un Proyecto
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProyectoViewModel modelo)
        {
            string uniqueFileName = "";
            if (ModelState.IsValid)
            {
                try
                {
                    Proyecto proyecto = new Proyecto
                    {
                        RutaKML = uniqueFileName,
                        Nombre = modelo.Nombre,
                        TipoGeograficoId = modelo.TipoGeograficoId,
                    };
                    if (modelo.KML != null)
                    {
                        // The image must be uploaded to the images folder in wwwroot
                        // To get the path of the wwwroot folder we are using the inject
                        // HostingEnvironment service provided by ASP.NET Core
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "kmz");
                        // To make sure the file name is unique we are appending a new
                        // GUID value and and an underscore to the file name
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + modelo.KML.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        modelo.KML.CopyTo(new FileStream(filePath, FileMode.Create));
                        modelo.RutaKML = uniqueFileName;
                        using (var ms = new MemoryStream())
                        {
                            modelo.KML.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            proyecto.KML = fileBytes;
                            proyecto.RutaKML = uniqueFileName;
                        }
                    }
                    _context.Add(proyecto);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este proyecto.");
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

            return View(modelo);
        }


        /// <summary>
        /// Permite editar un Proyecto
        /// </summary>
        /// <param name="id">Identifica el proyecto</param>
        /// <returns></returns>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proyectoViewModel = _context.Proyectos.Select(s => new ProyectoViewModel
            {
                Id = s.Id,
                RutaKML = s.RutaKML,
                Nombre = s.Nombre,
                TipoGeograficoId = s.TipoGeograficoId,
            }).Where(f => f.Id == id.Value).FirstOrDefault();

            if (proyectoViewModel == null)
            {
                return NotFound();
            }

            return View(proyectoViewModel);
        }

        /// <summary>
        /// Almacena los cambios dados por el usuario
        /// </summary>
        /// <param name="id">Identifica el proyectoa</param>
        /// <param name="proyecto">Objeto de Tipo Materia</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProyectoViewModel modelo)
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

                    Proyecto proyecto = _context.Proyectos.Find(id);

                    if (modelo.KML != null)
                    {
                        // The image must be uploaded to the images folder in wwwroot
                        // To get the path of the wwwroot folder we are using the inject
                        // HostingEnvironment service provided by ASP.NET Core
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "kmz");
                        // To make sure the file name is unique we are appending a new
                        // GUID value and and an underscore to the file name
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + modelo.KML.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        modelo.KML.CopyTo(new FileStream(filePath, FileMode.Create));
                        modelo.RutaKML = filePath;
                        using (var ms = new MemoryStream())
                        {
                            modelo.KML.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            proyecto.KML = fileBytes;
                            proyecto.RutaKML = uniqueFileName;
                        }
                    }
                    proyecto.Nombre = modelo.Nombre;



                    _context.Update(proyecto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este proyecto.");
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
            return View(modelo);
        }

        /// <summary>
        /// Permite eliminar un proyecto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Proyecto proyecto = _context.Proyectos
                .Select(x => new Proyecto
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    RutaKML = x.RutaKML,
                    TipoGeograficoId = x.TipoGeograficoId,
                })
                .Where(x => x.Id == id.Value)
                .FirstOrDefault();

            if (proyecto == null)
            {
                return NotFound();
            }

            _context.Proyectos.Remove(proyecto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Consulta el listado de países
        /// </summary>
        /// <returns></returns>
        public IActionResult Paises()
        {
            return View(_context.Paises.Select(s => new Pais
            {
                Id = s.Id,
                Nombre = s.Nombre,
            }).ToList());
        }

        /// <summary>
        /// Permite crear un Pais
        /// </summary>
        /// <returns></returns>
        public IActionResult CreatePais()
        {
            return View(new PaisViewModel { Nombre = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePais(PaisViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Pais pais = new Pais
                    {
                        Nombre = modelo.Nombre,
                    };

                    _context.Add(pais);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Paises));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este País.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch
                {
                    ModelState.AddModelError("Nombre", "Ya existe este País.");
                    return View(modelo);
                }
            }

            return View(modelo);
        }

        /// <summary>
        /// Permite editar un País
        /// </summary>
        /// <param name="id">Identifica el proyecto</param>
        /// <returns></returns>
        public IActionResult EditPais(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paisViewModel = _context.Paises.Select(s => new PaisViewModel
            {
                Id = s.Id,
                Nombre = s.Nombre,
            }).Where(f => f.Id == id.Value).FirstOrDefault();

            if (paisViewModel == null)
            {
                return NotFound();
            }

            return View(paisViewModel);
        }

        /// <summary>
        /// Almacena los cambios dados por el usuario
        /// </summary>
        /// <param name="id">Identifica el país</param>
        /// <param name="modelo">Objeto de Tipo Pais</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPais(int id, PaisViewModel modelo)
        {
            if (id != modelo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    Pais pais = _context.Paises.Find(id);

                    pais.Nombre = modelo.Nombre;


                    _context.Update(pais);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Paises));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este país.");
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
            return View(modelo);
        }

        /// <summary>
        /// Permite eliminar un país
        /// </summary>
        /// <param name="id">Identifica el país</param>
        /// <returns></returns>
        public async Task<IActionResult> DeletePais(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pais pais = _context.Paises
                .Select(x => new Pais
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                })
                .Where(x => x.Id == id.Value)
                .FirstOrDefault();

            if (pais == null)
            {
                return NotFound();
            }

            _context.Paises.Remove(pais);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Paises));
        }


        /// <summary>
        /// Consulta el listado de Tipos de Fuentes Bibliográficas
        /// </summary>
        /// <returns></returns>
        public IActionResult TiposFuenteBibliograficas()
        {
            return View(_context.TiposFuenteBibliograficas.Select(s => new TipoFuenteBibliografica
            {
                Id = s.Id,
                Nombre = s.Nombre,
            }).OrderBy(o => o.Nombre).ToList());
        }

        /// <summary>
        /// Permite crear un Tipo de Fuente Bibliografica
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateFuenteBibliografica()
        {
            return View(new TipoFuenteBibliograficaViewModel { Nombre = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFuenteBibliografica(TipoFuenteBibliograficaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TipoFuenteBibliografica tipoFteB = new TipoFuenteBibliografica
                    {
                        Nombre = modelo.Nombre,
                    };

                    _context.Add(tipoFteB);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(TiposFuenteBibliograficas));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este Tipo de Fuente Bibliográfica.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch
                {
                    ModelState.AddModelError("Nombre", "Ya existe este Tipo de Fuente Bibliográfica.");
                    return View(modelo);
                }
            }

            return View(modelo);
        }

        /// <summary>
        /// Permite editar una Fuente Bibliográfica
        /// </summary>
        /// <param name="id">Identifica el el Tipo de Fuente Bibliográfica</param>
        /// <returns></returns>
        public IActionResult EditFuenteBibliografica(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = _context.TiposFuenteBibliograficas.Select(s => new TipoFuenteBibliograficaViewModel
            {
                Id = s.Id,
                Nombre = s.Nombre,
            }).Where(f => f.Id == id.Value).FirstOrDefault();

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        /// <summary>
        /// Almacena los cambios dados por el usuario
        /// </summary>
        /// <param name="id">Identifica el Tipo de Fuente Bibliográfica</param>
        /// <param name="modelo">Objeto de Tipo Pais</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFuenteBibliografica(int id, TipoFuenteBibliograficaViewModel modelo)
        {
            if (id != modelo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    TipoFuenteBibliografica tipoFte = _context.TiposFuenteBibliograficas.Find(id);

                    tipoFte.Nombre = modelo.Nombre;


                    _context.Update(tipoFte);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(TiposFuenteBibliograficas));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este Tipo de Fuente Bibliográfica.");
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
            return View(modelo);
        }

        /// <summary>
        /// Permite eliminar un Tipo de Fuente Bibliográfica
        /// </summary>
        /// <param name="id">Identifica el Tipo de Fuente Bibliográfica</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteFuenteBibliografica(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TipoFuenteBibliografica tipoFte = _context.TiposFuenteBibliograficas
                .Select(x => new TipoFuenteBibliografica
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                })
                .Where(x => x.Id == id.Value)
                .FirstOrDefault();

            if (tipoFte == null)
            {
                return NotFound();
            }

            _context.TiposFuenteBibliograficas.Remove(tipoFte);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TiposFuenteBibliograficas));
        }

        /// <summary>
        /// Consulta el listado de Tipos de Documentos
        /// </summary>
        /// <returns></returns>
        public IActionResult TiposDocumentos()
        {
            return View(_context.TiposDocumento.Select(s => new TipoDocumento
            {
                Id = s.Id,
                Nombre = s.Nombre,
            }).OrderBy(o => o.Nombre).ToList());
        }

        /// <summary>
        /// Permite crear un Tipo de Documento
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateTipoDocumento()
        {
            return View(new TipoDocumentoViewModel { Nombre = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTipoDocumento(TipoDocumentoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TipoDocumento tipoDocumento = new TipoDocumento
                    {
                        Nombre = modelo.Nombre,
                    };

                    _context.Add(tipoDocumento);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(TiposDocumentos));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este Tipo de Documento.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch
                {
                    ModelState.AddModelError("Nombre", "Ya existe este Tipo de Documento.");
                    return View(modelo);
                }
            }

            return View(modelo);
        }

        /// <summary>
        /// Permite editar un Tipo de Documento
        /// </summary>
        /// <param name="id">Identifica el el Tipo de Documento</param>
        /// <returns></returns>
        public IActionResult EditTipoDocumento(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = _context.TiposDocumento.Select(s => new TipoDocumentoViewModel
            {
                Id = s.Id,
                Nombre = s.Nombre,
            }).Where(f => f.Id == id.Value).FirstOrDefault();

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        /// <summary>
        /// Almacena los cambios dados por el usuario
        /// </summary>
        /// <param name="id">Identifica el Tipo de Documento</param>
        /// <param name="modelo">Objeto de Tipo TipoDocumentoViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTipoDocumento(int id, TipoDocumentoViewModel modelo)
        {
            if (id != modelo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    TipoDocumento tipoDocumento = _context.TiposDocumento.Find(id);

                    tipoDocumento.Nombre = modelo.Nombre;


                    _context.Update(tipoDocumento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(TiposDocumentos));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este Tipo de Documento.");
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
            return View(modelo);
        }


        /// <summary>
        /// Permite eliminar un Tipo de Documento
        /// </summary>
        /// <param name="id">Identifica el Tipo de Documento</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteTipoDocumento(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TipoDocumento tipoDocumento = _context.TiposDocumento
                .Select(x => new TipoDocumento
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                })
                .Where(x => x.Id == id.Value)
                .FirstOrDefault();

            if (tipoDocumento == null)
            {
                return NotFound();
            }

            _context.TiposDocumento.Remove(tipoDocumento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TiposDocumentos));
        }

        /// <summary>
        /// Lista las fotos de la galería vinculadas con el proyecto
        /// </summary>
        /// <param name="id">Identifica el proyecto</param>
        /// <returns></returns>
        public IActionResult GaleriaProyecto(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proyecto = this.ObtenerProyectosConFotos(id.Value);
            if (proyecto == null)
            {
                return NotFound();
            }
            return View(proyecto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Identifica el proyecto</param>
        /// <returns></returns>
        private Proyecto ObtenerProyectosConFotos(int id)
        {
            try
            {
                var proyecto = this._context.Proyectos
                .Where(c => c.Id == id)
                .Select(s => new Proyecto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                })
                .FirstOrDefault();

                proyecto.GaleriaImagenesProyectos = this._context.GaleriaImagenesProyectos.
                    Where(f => f.ProyectoId == id)
                    .Select(s =>  new GaleriaImagenesProyecto
                    {
                        Id = s.Id,
                        ProyectoId = s.ProyectoId,
                        Descripcion = s.Descripcion,
                    })
                    .ToList();


                return proyecto;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Permite vincular una Foto a la Galeria de fotos del proyecto
        /// </summary>
        /// <returns></returns>
        public IActionResult VincularFoto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VincularFoto(GaleriaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    GaleriaImagenesProyecto galeriaProyecto = new GaleriaImagenesProyecto
                    {
                        Descripcion = modelo.Descripcion,
                        ProyectoId = modelo.ProyectoId,
                    };
                    if (modelo.Foto != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            modelo.Foto.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            galeriaProyecto.Foto = fileBytes;
                            
                        }
                    }
                    _context.Add(galeriaProyecto);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este foto vinculad a la galería.");
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

            return View(modelo);
        }
    }
}
