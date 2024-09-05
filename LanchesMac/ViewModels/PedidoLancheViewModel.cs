using LanchesMac.Models;

namespace LanchesMac.ViewModels
{
    public class PedidoLancheViewModel
    {
        public Pedido Pedido { get; set; }

        public IEnumerable<PedidoDetalhe> PedidoDetalhe {  get; set; }
    }
}
