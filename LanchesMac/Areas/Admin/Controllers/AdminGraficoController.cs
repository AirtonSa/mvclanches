using LanchesMac.Areas.Admin.Servicos;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LanchesMac.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AdminGraficoController : Controller
    {
        private readonly GraficoVendasService _graficovendas;

        public AdminGraficoController(GraficoVendasService graficovendas)
        {
            _graficovendas = graficovendas;
        }

        [HttpGet]
        public JsonResult VendasLanches(int dias)
        {
            var lanchesVendasTotais = _graficovendas.GetVendasLanches(dias);
            return Json(lanchesVendasTotais);
        }
        [HttpGet]
        public IActionResult Index() //calcular as vendas dos ultimos 360 dias
        {
            return View();
        }
        [HttpGet]
        public IActionResult VendasMensal() //calcular as venddas mensais
        {
            return View();
        }
        [HttpGet]
        public IActionResult VendasSemanal() //calcular as vendas semanais
        {
            return View();
        }
    }
}

