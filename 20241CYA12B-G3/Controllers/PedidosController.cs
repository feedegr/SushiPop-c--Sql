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
        public async Task<IActionResult> ConfirmarPdido()
        {

            var user = await _userManager.GetUserAsync(User);

            var carrito = await _context.Carrito
            .Include(c => c.Cliente)
            .Include(c => c.CarritoItems)
            .ThenInclude(ci => ci.Producto)
            .FirstOrDefaultAsync(c => c.Cliente.Email.ToUpper() == user.NormalizedEmail && c.Procesado == false);

            //Traer pedidos de los últimos 30dias
            var fechaInicio = DateTime.Now.AddDays(-30);
            var pedidosRecientes = await _context.Pedido
                .Include(p => p.Carrito)
                .ThenInclude(c => c.Cliente)
                .Where(p => p.FechaCompra >= fechaInicio && p.Carrito.Cliente.Email.ToUpper() == user.NormalizedEmail)
                .ToListAsync();


            decimal gastoEnvio;

                if (pedidosRecientes.Count >= 10)
                {
                    gastoEnvio = 50;
                }

            


            var Subtotal = carrito.CarritoItems.Sum(ci => ci.PrecioUnitarioConDescuento * ci.Cantidad);
            var pedido = new Pedido

            {       //  CORREGIR¡¡ 
                    //CarritoId = carrito.Id,
                    //FechaCompra = DateTime.Now,
                    //Subtotal = Subtotal,
                    //GastoEnvio = gastoEnvio,
                    //Total = Subtotal + gastoEnvio,
                    //Estado = 1
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

            decimal subtotal = carrito.CarritoItems.Sum(ci => ci.PrecioUnitarioConDescuento * ci.Cantidad);
            decimal gastoEnvio = 50;
            //TO DO FALTA CALCULAR GASTO ENVIO
            DetallePedidoViewModel vm = new DetallePedidoViewModel()

            {
                Cliente = carrito.Cliente.Nombre + " " + carrito.Cliente.Apellido,
                Direccion = carrito.Cliente.Direccion,
                Subtotal = subtotal,
                Total = subtotal + gastoEnvio,
                Productos = carrito.CarritoItems.Select(ci => ci.Producto.Nombre).ToList()
            };


            return View("DetallePedido",  vm);
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
        [Authorize(Roles = "CLIENTE")]
        [Authorize(Roles = "EMPLEADO")]
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

        private bool ValidarNumeroPedido(int numeroPedido)
        {
            
            var pedido = _context.Pedido.FirstOrDefault(p => p.NroPedido == numeroPedido);

            if (pedido == null)
            {
               
                TempData["ErrorMessage"] = $"El número pedido {numeroPedido} no es correcto.";
                return false;
            }

            return true;
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
