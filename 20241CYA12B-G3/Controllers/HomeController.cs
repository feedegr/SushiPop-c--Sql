using _20241CYA12B_G3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _20241CYA12B_G3.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbContext _context;

        public HomeController(DbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //ver que numero de dia es
            var nroDia = (int)DateTime.Today.DayOfWeek;


            //nombre del dia
            var nombreDelDia = System.Globalization.CultureInfo.GetCultureInfo("es-ES").DateTimeFormat.GetDayName((DayOfWeek)nroDia);

            HomeViewModel homeViewModel = new()
            {

                NombreDia = nombreDelDia,
                Descuento = "35%",
                Producto = "Combo Alaska 25 piezas"
            };


            



            return View(homeViewModel);
        }

       

        
    }
}
