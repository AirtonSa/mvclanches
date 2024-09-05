
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private readonly ILancheRepository _lancheRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraController(ILancheRepository lancheRepository, CarrinhoCompra carrinhoCompra)
        {
            _lancheRepository = lancheRepository;  
            _carrinhoCompra = carrinhoCompra;

        }
        public IActionResult Index()
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItens(); //pegar os itens do carrinho no CarrinhoCompraItens(Lista)
            itens = _carrinhoCompra.CarrinhoCompraItens; //aqui vou atribuir os itens que peguei na variavel Itens 

            var carrinhoCompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
            };
            return View(carrinhoCompraVM);
        }
        public IActionResult AdicionarItemNoCarrinhoCompra(int lancheId)
        {
            var adicionarLanche = _lancheRepository.Lanches.FirstOrDefault(l => l.LancheId == lancheId);

            if (adicionarLanche != null)
            {
                _carrinhoCompra.AdicionarCarrinho(adicionarLanche);
            }
            return RedirectToAction("Index");
        }
        
        public IActionResult RemoverItemDoCarrinhoDeCompra(int lancheId)
        {
            var Remover = _lancheRepository.Lanches.FirstOrDefault(l => l.LancheId == lancheId);
            if (Remover != null)
            {
                _carrinhoCompra.RemoverDoCarrinho(Remover);
            }

            return RedirectToAction("Index");
        }
    }
}
