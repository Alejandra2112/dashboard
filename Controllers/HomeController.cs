using dashboard.Models.ViewModels;
using dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly DashboardContext _dbContext;

        public HomeController(DashboardContext context)
        {
            _dbContext = context;
        }
     

        public IActionResult resumenVenta() //metodo para obtener informacion de las ventas de los ultimos 5 dias
        {
            DateTime fechaInicio = DateTime.Now; //Fecha actual
            fechaInicio = fechaInicio.AddDays(-5); //dias de los que se va obtener el resumen

            List<VMVenta> Lista = (from tbventa in _dbContext.Venta
                                   where tbventa.FechaRegistro.Value.Date >= fechaInicio.Date
                                   group tbventa by tbventa.FechaRegistro.Value.Date into grupo
                                   select new VMVenta
                                   {
                                       fecha = grupo.Key.ToString("dd/MM/yy"),
                                       cantidad = grupo.Count(),
                                   }).ToList();
            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        public IActionResult resumenProducto()  //metodo para obtener informacion de los productos mas vendidos
        {

            List<VMProducto> Lista = (from tbdetalleVenta in _dbContext.DetalleVenta
                                      group tbdetalleVenta by tbdetalleVenta.NombreProducto into grupo
                                      orderby grupo.Count() descending
                                      select new VMProducto
                                      {
                                          producto = grupo.Key,
                                          cantidad = grupo.Count(),
                                      }).Take(4).ToList();
            return StatusCode(StatusCodes.Status200OK, Lista);
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}