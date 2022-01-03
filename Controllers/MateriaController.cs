using GeoPlus.Data;
using GeoPlus.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPlus.Controllers
{
    /// <summary>
    /// Controlador Materia
    /// </summary>
    //[Authorize(Roles = "Admin")]
    public class MateriaController : Controller
    {
        private readonly DataContext _context;

        public MateriaController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna el listado de materias
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(_context.Materias.ToList());
        }

        /// <summary>
        /// Permite crear una Materia
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Materia modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(modelo);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este descriptor.");
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
        /// Permite editar una Materia
        /// </summary>
        /// <param name="id">Identifica la Materia</param>
        /// <returns></returns>
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Materia materia = _context.Materias.Find(id);
            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        /// <summary>
        /// Almacena los cambios dados por el usuario
        /// </summary>
        /// <param name="id">Identifica la materia</param>
        /// <param name="materia">Objeto de Tipo Materia</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Materia materia)
        {
            if (id != materia.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materia);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe este descriptor.");
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
            return View(materia);
        }

        /// <summary>
        /// Permite eliminar una materia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Materia materia = _context.Materias
                    .FirstOrDefault(m => m.Id == id);
                if (materia == null)
                {
                    return NotFound();
                }

                _context.Materias.Remove(materia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(NoDelete));
            }
        }

        
        public IActionResult NoDelete()
        {
            return View();
        }
    }
}
