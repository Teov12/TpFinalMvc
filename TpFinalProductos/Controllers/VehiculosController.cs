using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Text;
using TpFinalProductos.Data;
using TpFinalProductos.Models;
using TpFinalProductos.ModelsView;

namespace TpFinalProductos.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment env;

        public VehiculosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

       


        // GET: Vehiculos
        public async Task<IActionResult> Index(string busquedaNombre, string busquedaMarca, string busquedaCategoria, int pagina = 1)
        {
            ViewData["categoriaId"] = new SelectList(_context.Categorias, "Id", "Id");
            ViewData["marcaId"] = new SelectList(_context.Marcas, "Id", "Id");
            ViewData["tipoDeVehiculoId"] = new SelectList(_context.TipoDeVehiculos, "Id", "Id");
            paginador paginador = new paginador()
            {
                pagActual = pagina,
                regXpag = 5
            };

            var consulta = _context.Vehiculos.Include(v => v.Categoria).Include(v => v.Marca).Include(v => v.TipoVehiculo).Select(v => v);

            if (!string.IsNullOrEmpty(busquedaNombre)) {

                 consulta = consulta.Where(v => v.nombre.Contains(busquedaNombre));
            }
            if (!string.IsNullOrEmpty(busquedaMarca))
            {

                consulta = consulta.Where(e => e.Marca.Nombre.Contains(busquedaMarca));
            }
            if (!string.IsNullOrEmpty(busquedaCategoria))
            {

                consulta = consulta.Where(e => e.Categoria.descripcion.Contains(busquedaCategoria));
            }

            paginador.cantReg = consulta.Count();

            var datosAmostrar = consulta
                .Skip((paginador.pagActual - 1) * paginador.regXpag)
                .Take(paginador.regXpag);

            foreach (var item in Request.Query)
                paginador.ValoresQueryString.Add(item.Key, item.Value);

            VehiculosViewModel Datos = new VehiculosViewModel()
            {
                ListaVehiculos = datosAmostrar.ToList(),
                busquedaNombre = busquedaNombre,
                busquedaMarca = busquedaMarca,
                busquedaCategoria = busquedaCategoria,
                paginador = paginador
            };


            return View(Datos);
        }

        // GET: Vehiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.Categoria)
                .Include(v => v.Marca)
                .Include(v => v.TipoVehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // GET: Vehiculos/Create
        public IActionResult Create()
        {
            ViewData["categoriaId"] = new SelectList(_context.Categorias, "Id", "descripcion");
            ViewData["marcaId"] = new SelectList(_context.Marcas, "Id", "Nombre");
            ViewData["tipoDeVehiculoId"] = new SelectList(_context.TipoDeVehiculos, "Id", "vehiculo");
            return View();
        }

        // POST: Vehiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,tipoDeVehiculoId,marcaId,nombre,precio,categoriaId,resumen,foto")] Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivofoto = archivos[0];

                    if (archivofoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(env.WebRootPath, "images/vehiculos");
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivofoto.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivofoto.CopyTo(filestream);
                            vehiculo.foto = archivoDestino;
                        }
                    }
                }
                _context.Add(vehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["categoriaId"] = new SelectList(_context.Categorias, "Id", "descripcion");
            ViewData["marcaId"] = new SelectList(_context.Marcas, "Id", "Nombre");
            ViewData["tipoDeVehiculoId"] = new SelectList(_context.TipoDeVehiculos, "Id", "vehiculo");
            return View(vehiculo);
        }

        // GET: Vehiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            ViewData["categoriaId"] = new SelectList(_context.Categorias, "Id", "descripcion");
            ViewData["marcaId"] = new SelectList(_context.Marcas, "Id", "Nombre");
            ViewData["tipoDeVehiculoId"] = new SelectList(_context.TipoDeVehiculos, "Id", "vehiculo");
            return View(vehiculo);
        }

        // POST: Vehiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,tipoDeVehiculoId,marcaId,nombre,precio,categoriaId,resumen,foto")] Vehiculo vehiculo)
        {
            if (id != vehiculo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivofoto = archivos[0];

                    if (archivofoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(env.WebRootPath, "images/vehiculos");
                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivofoto.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        if (!string.IsNullOrEmpty(vehiculo.foto))
                        {
                            string fotoAnterior = Path.Combine(pathDestino, vehiculo.foto);
                            if (System.IO.File.Exists(fotoAnterior))
                                System.IO.File.Delete(fotoAnterior);
                        }

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivofoto.CopyTo(filestream);
                            vehiculo.foto = archivoDestino;
                        }
                    }
                }
                try
                {
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculoExists(vehiculo.Id))
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
            ViewData["categoriaId"] = new SelectList(_context.Categorias, "Id", "descripcion");
            ViewData["marcaId"] = new SelectList(_context.Marcas, "Id", "Nombre");
            ViewData["tipoDeVehiculoId"] = new SelectList(_context.TipoDeVehiculos, "Id", "vehiculo");
            return View(vehiculo);
        } 

        // GET: Vehiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.Categoria)
                .Include(v => v.Marca)
                .Include(v => v.TipoVehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // POST: Vehiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculoExists(int id)
        {
            return _context.Vehiculos.Any(e => e.Id == id);
        }
    }
}
