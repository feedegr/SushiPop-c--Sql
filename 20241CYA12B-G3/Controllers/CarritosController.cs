using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _20241CYA12B_G3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

namespace _20241CYA12B_G3.Controllers
{
    public class CarritosController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CarritosController(DbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Carritos
        [Authorize(Roles = "CLIENTE")]
        public async Task<IActionResult> Index()
        {
            var dbContext = _context.Carrito.Include(c => c.Cliente);
            return View(await dbContext.ToListAsync());
        }

        [Authorize(Roles = "CLIENTE")]
        public async Task<IActionResult> AgregarEditarProducto(int productoId)
        {
            var producto = await _context.Producto.FindAsync(productoId);

            if (producto.Stock < 1)
            {
                return NotFound();
            }


            var user = await _userManager.GetUserAsync(User);

            var clienteId = _context.Cliente.FirstOrDefault(c => c.Email.ToUpper() == user.Email).Id;


            var pedido = await _context.Pedido.Include(p => p.Carrito).FirstOrDefaultAsync(p => p.Carrito.ClienteId == clienteId && p.Estado == 1);

            if(pedido != null)
            {
                return NotFound();
            }

            var cantPedidos = await _context.Pedido.Include(p => p.Carrito).Where(p => p.Carrito.ClienteId == clienteId && p.FechaCompra == DateTime.Today).ToListAsync();

            if(cantPedidos.Count > 3)
            {
                return NotFound();
            }

            var carrito = await _context.Carrito.Include(c => c.Cliente).Include(c => c.CarritoItems).FirstOrDefaultAsync(c => c.ClienteId == clienteId);

            if(carrito == null)
            {
                carrito = new Carrito()
                {
                    Procesado = false,
                    Cancelado = false,
                    ClienteId = clienteId,
                    CarritoItems = new List<CarritoItem>()
               
                };

                _context.Add(carrito);
                await _context.SaveChangesAsync();
            }

            var item = carrito.CarritoItems.FirstOrDefault(ci => ci.ProductoId == productoId);

            //Buscar el descuento del producto

            var nroDia = (int)DateTime.Today.DayOfWeek;

            var descuento = await _context.Descuento.FirstOrDefaultAsync(d => d.Dia == nroDia && d.Activo == true && d.ProductoId == productoId);

            // VER SI EL DESCUENTO EXISTE Y APLICARLO...

            if(descuento == null)
            {
                _context.Add(item);
                item.Cantidad--;
                await _context.SaveChangesAsync();
            }
            else
            {



            }


            if(item == null)
            {

                item = new CarritoItem()
                {
                    PrecioUnitarioConDescuento = producto.Precio,
                    Cantidad = 1,
                    CarritoId = carrito.Id,
                    ProductoId = producto.Id,

            };  
                    _context.Add(item);
                    await _context.SaveChangesAsync();

            }
            else
            {
                item.Cantidad++;
                _context.Update(item);
                await _context.SaveChangesAsync();
            }


            producto.Stock--;
            _context.Update(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Carritos");
        }



        // GET: Carritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carrito == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carrito
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // GET: Carritos/Create
        [Authorize(Roles = "CLIENTE")]
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id");
            return View();
        }

        // POST: Carritos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Procesado,Cancelado,ClienteId")] Carrito carrito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", carrito.ClienteId);
            return View(carrito);
        }

        // GET: Carritos/Edit/5
        [Authorize(Roles = "CLIENTE")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carrito == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carrito.FindAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", carrito.ClienteId);
            return View(carrito);
        }

        // POST: Carritos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Procesado,Cancelado,ClienteId")] Carrito carrito)
        {
            if (id != carrito.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarritoExists(carrito.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Set<Cliente>(), "Id", "Id", carrito.ClienteId);
            return View(carrito);
        }

        // GET: Carritos/Delete/5
        [Authorize(Roles = "CLIENTE")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carrito == null)
            {
                return NotFound();
            }

            var carrito = await _context.Carrito
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carrito == null)
            {
                return NotFound();
            }

            return View(carrito);
        }

        // POST: Carritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carrito == null)
            {
                return Problem("Entity set 'DbContext.Carrito'  is null.");
            }
            var carrito = await _context.Carrito.FindAsync(id);
            if (carrito != null)
            {
                _context.Carrito.Remove(carrito);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarritoExists(int id)
        {
          return (_context.Carrito?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
