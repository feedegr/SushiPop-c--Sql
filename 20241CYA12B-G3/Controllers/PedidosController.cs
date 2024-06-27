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

namespace _20241CYA12B_G3.Controllers
{
    public class PedidosController : Controller
    {
        private readonly DbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PedidosController(DbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // Confirmar Pedido
        public async Task<IActionResult> ConfirmarPedido()
        {

            var user = await _userManager.GetUserAsync(User);

            var carrito = await _context.Carrito
            .Include(c => c.Cliente)
            .Include(c => c.CarritoItems)
            .ThenInclude(ci => ci.Producto)
            .FirstOrDefaultAsync(c => c.Cliente.Email.ToUpper() == user.NormalizedEmail && c.Procesado == false);

            if (carrito == null)
            {
                return NotFound("No se encontró el carrito del usuario.");
            }

            decimal subtotal = carrito.CarritoItems.Sum(ci => ci.PrecioUnitarioConDescuento * ci.Cantidad);
            decimal gastoEnvio = 80;

            var fechaInicio = DateTime.Now.AddDays(-30);
            var pedidosEntregadosRecientes = await _context.Pedido
                .Where(p => p.Estado == 5 && p.FechaCompra >= fechaInicio && p.Carrito.Cliente.Email.ToUpper() == user.NormalizedEmail)
                .ToListAsync();


            if (pedidosEntregadosRecientes.Count >= 10)
            {
                gastoEnvio = 0; 
            }

            var pedido = new Pedido
            {
                CarritoId = carrito.Id,
                FechaCompra = DateTime.Now,
                Subtotal = subtotal,
                GastoEnvio = gastoEnvio,
                Total = subtotal + gastoEnvio,
                Estado = 1 
            };

            _context.Add(pedido);
            await _context.SaveChangesAsync();

            carrito.Procesado = true;
            _context.Update(carrito);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles ="CLIENTE")]
        public async Task<IActionResult> HacerPedido(int idCarrito)
        {
            var carrito = await _context.Carrito
               .Include(c => c.Cliente)
               .Include(c => c.CarritoItems)
               .ThenInclude(ci => ci.Producto)
               .FirstOrDefaultAsync(c => c.Id == idCarrito);

            if (carrito == null)
            {
                return NotFound("Carrito no encontrado.");
            }

            decimal subtotal = carrito.CarritoItems.Sum(ci => ci.PrecioUnitarioConDescuento * ci.Cantidad);
            decimal gastoEnvio = 80; 

            var user = await _userManager.GetUserAsync(User);
            var fechaInicio = DateTime.Now.AddDays(-30);
            var pedidosEntregadosRecientes = await _context.Pedido
                .Where(p => p.Estado == 5 && p.FechaCompra >= fechaInicio && p.Carrito.Cliente.Email.ToUpper() == user.NormalizedEmail)
                .ToListAsync();

            if (pedidosEntregadosRecientes.Count >= 10)
            {
                gastoEnvio = 0; 
            }

            DetallePedidoViewModel vm = new DetallePedidoViewModel
            {
                Cliente = carrito.Cliente.Nombre + " " + carrito.Cliente.Apellido,
                Direccion = carrito.Cliente.Direccion,
                Subtotal = subtotal,
                Total = subtotal + gastoEnvio,
                Productos = carrito.CarritoItems.Select(ci => ci.Producto.Nombre).ToList()
            };

            return View("DetallePedido", vm);
        }

        [Authorize(Roles = "CLIENTE")]
        public async Task<IActionResult> CancelarPedido (int idCarrito)
        
        {
            try
            {
                var carritoCancelar = await _context.Carrito.FindAsync(idCarrito);
                carritoCancelar.Cancelado = true;
                _context.Update(carritoCancelar);
                await _context.SaveChangesAsync();
                return StatusCode(202);
            }
            catch 
            {
                return StatusCode(304);
            }
           
        } 

        //TO DO METODO CONFIRMAR DE DETALLE PEDIDO

        // GET: Pedidos
        [Authorize(Roles = "CLIENTE,EMPLEADO")]
        public async Task<IActionResult> Index()
        {
            var dbContext = _context.Pedido.Include(p => p.Carrito);
            return View(await dbContext.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Carrito)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }


        // GET: Pedidoes/Create
        [Authorize(Roles = "CLIENTE")]
        public IActionResult Create()
        {
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "Id", "Id");
            return View();
        }

        // POST: Pedidoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NroPedido,FechaCompra,Subtotal,GastoEnvio,Total,Estado,CarritoId")] Pedido pedido)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "Id", "Id", pedido.CarritoId);
            return View(pedido);
        }

        // GET: Pedidoes/Edit/5
        [Authorize(Roles = "EMPLEADO")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "Id", "Id", pedido.CarritoId);
            return View(pedido);
        }

        // POST: Pedidoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NroPedido,FechaCompra,Subtotal,GastoEnvio,Total,Estado,CarritoId")] Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidoExists(pedido.Id))
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
            ViewData["CarritoId"] = new SelectList(_context.Carrito, "Id", "Id", pedido.CarritoId);
            return View(pedido);
        }

        // GET: Pedidoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pedido == null)
            {
                return NotFound();
            }

            var pedido = await _context.Pedido
                .Include(p => p.Carrito)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // POST: Pedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pedido == null)
            {
                return Problem("Entity set 'DbContext.Pedido'  is null.");
            }
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedido.Remove(pedido);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidoExists(int id)
        {
          return (_context.Pedido?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
