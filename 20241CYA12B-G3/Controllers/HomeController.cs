﻿using _20241CYA12B_G3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult>Index()
        {
          
            var nroDia = (int)DateTime.Today.DayOfWeek;

            var nombreDelDia = System.Globalization.CultureInfo.GetCultureInfo("es-ES").DateTimeFormat.GetDayName((DayOfWeek)nroDia);

            var descuento = await _context.Descuento.Include(d => d.Producto).FirstOrDefaultAsync(d => d.Dia == nroDia && d.Activo);

            
            HomeViewModel homeViewModel = new();

            if (descuento == null)
            {

                homeViewModel.MensajeHero = "Hoy es " + nombreDelDia + " Disfruta del mejor sushi #EnCasa con amigos";

            }
            else
            {

                homeViewModel.NombreDia = nombreDelDia;
                homeViewModel.Descuento = descuento.Porcentaje + " %";
                homeViewModel.Producto = descuento.Producto.Nombre;
           
            }


            return View(homeViewModel);
        }

       

        
    }
}
