﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _20241CYA12B_G3.Models;
using Microsoft.AspNetCore.Authorization;

namespace _20241CYA12B_G3.Controllers
{
    public class DescuentosController : Controller
    {
        private readonly DbContext _context;

        public DescuentosController(DbContext context)
        {
            _context = context;
        }

        // GET: Descuentos
        [Authorize(Roles = "EMPLEADO")]
        public async Task<IActionResult> Index()
        {
            var dbContext = _context.Descuento.Include(d => d.Producto);
            return View(await dbContext.ToListAsync());
        }

        // GET: Descuentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Descuento == null)
            {
                return NotFound();
            }

            var descuento = await _context.Descuento
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (descuento == null)
            {
                return NotFound();
            }

            return View(descuento);
        }

        // GET: Descuentos/Create
        [Authorize(Roles = "EMPLEADO")]
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Set<Producto>(), "Id", "Id");
            return View();
        }

        // POST: Descuentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dia,Porcentaje,DescuentoMaximo,Activo,ProductoId")] Descuento descuento)
        {
            if (descuento.Porcentaje > 50)
            {
                ModelState.AddModelError("Porcentaje", "El porcentaje de descuento no puede ser mayor al 50%.");
            }
            if (descuento.DescuentoMaximo > 3000)
            {
                ModelState.AddModelError("DescuentoMaximo", "El descuento máximo no puede ser mayor a $3000.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(descuento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Set<Producto>(), "Id", "Id", descuento.ProductoId);
            return View(descuento);
        }

        // GET: Descuentos/Edit/5
        [Authorize(Roles = "EMPLEADO")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Descuento == null)
            {
                return NotFound();
            }

            var descuento = await _context.Descuento.FindAsync(id);
            if (descuento == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Set<Producto>(), "Id", "Id", descuento.ProductoId);
            return View(descuento);
        }

        // POST: Descuentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Dia,Porcentaje,DescuentoMaximo,Activo,ProductoId")] Descuento descuento)
        {
            if (id != descuento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(descuento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DescuentoExists(descuento.Id))
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
            ViewData["ProductoId"] = new SelectList(_context.Set<Producto>(), "Id", "Id", descuento.ProductoId);
            return View(descuento);
        }

        // GET: Descuentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Descuento == null)
            {
                return NotFound();
            }

            var descuento = await _context.Descuento
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (descuento == null)
            {
                return NotFound();
            }

            return View(descuento);
        }

        // POST: Descuentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Descuento == null)
            {
                return Problem("Entity set 'DbContext.Descuento'  is null.");
            }
            var descuento = await _context.Descuento.FindAsync(id);
            if (descuento != null)
            {
                _context.Descuento.Remove(descuento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DescuentoExists(int id)
        {
          return (_context.Descuento?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
