using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoController(IPedidoRepository pedidoRepository, CarrinhoCompra carrinhocompra)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhocompra;
        }

        [HttpGet] //aqui será para demonstrar os dados para o cliente
        public IActionResult Checkout()
        {
            return View();

        }

        [HttpPost] //aqui será para pegar os dados do cliente
        public IActionResult Checkout(Pedido pedido)
        {
            int totalItensPedido = 0;
            decimal precoTotalPedido = 0.0m;
            //obter os itens do carrinho de compra do cliente
            List<CarrinhoCompraItem> items = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItens = items;

            // verificar se existem itens do pedido
            if (_carrinhoCompra.CarrinhoCompraItens.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho Esta vazio, que tal incluir um lanche...");
            }
            //calcular o total de itens eo total do pedido
            foreach(var item in items)
            {
                totalItensPedido += item.Quantidade;
                precoTotalPedido += (item.Lanche.Preco * item.Quantidade);
            }

            //atribui os valores obtidos ao pedido
            pedido.TotalItensPedido = totalItensPedido;
            pedido.PedidoTotal = precoTotalPedido;

            //valida os dados do pedido
            if (ModelState.IsValid)
            {
                //criar os pedido e os detalhes
                _pedidoRepository.CriarPedido(pedido);

                //define mensagem ao cliente
                ViewBag.CheckoutcompletoMensagem = "Obrigado pelo seu pedido :)";
                ViewBag.TotalPedido = -_carrinhoCompra.GetCarrinhoCompraTotal();

                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }

            return View(pedido);
        }

    }
}
