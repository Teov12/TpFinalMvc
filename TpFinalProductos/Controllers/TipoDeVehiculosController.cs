using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TpFinalProductos.Data;
using TpFinalProductos.Models;
using TpFinalProductos.ModelsView;

namespace TpFinalProductos.Controllers
{
    public class TipoDeVehiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoDeVehiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TipoDeVehiculos
        [Authorize]
        public async Task<IActionResult> Index(int pagina = 1)
        {
            paginador paginador = new paginador()
            {
                cantReg = _context.TipoDeVehiculos.Count(),
                pagActual = pagina,
                regXpag = 1
            };
            ViewData["paginador"] = paginador;

            var datosAmostrar = _context.TipoDeVehiculos
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            return View(await datosAmostrar.ToListAsync());
        }

        // GET: TipoDeVehiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeVehiculo = await _context.TipoDeVehiculos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoDeVehiculo == null)
            {
                return NotFound();
            }

            return View(tipoDeVehiculo);
        }

        // GET: TipoDeVehiculos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDeVehiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,vehiculo")] TipoDeVehiculo tipoDeVehiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoDeVehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDeVehiculo);
        }

        // GET: TipoDeVehiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeVehiculo = await _context.TipoDeVehiculos.FindAsync(id);
            if (tipoDeVehiculo == null)
            {
                return NotFound();
            }
            return View(tipoDeVehiculo);
        }

        // POST: TipoDeVehiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,vehiculo")] TipoDeVehiculo tipoDeVehiculo)
        {
            if (id != tipoDeVehiculo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoDeVehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoDeVehiculoExists(tipoDeVehiculo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDeVehiculo);
        }

        // GET: TipoDeVehiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDeVehiculo = await _context.TipoDeVehiculos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoDeVehiculo == null)
            {
                return NotFound();
            }

            return View(tipoDeVehiculo);
        }

        // POST: TipoDeVehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipoDeVehiculo = await _context.TipoDeVehiculos.FindAsync(id);
            _context.TipoDeVehiculos.Remove(tipoDeVehiculo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoDeVehiculoExists(int id)
        {
            return _context.TipoDeVehiculos.Any(e => e.Id == id);
        }
    }
}
