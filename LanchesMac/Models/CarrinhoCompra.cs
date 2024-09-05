using LanchesMac.Context;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Models
{
    public class CarrinhoCompra
    {

        private readonly AppDbContext _context; //isso daqui serve para acessar as tabelas do banco de dados para meche-las e fazer qualquer coisa numa tabela

        public CarrinhoCompra(AppDbContext context)//sempre fazer o construtor do Contex de cima
        {
            _context = context;
        }

        public string CarrinhoCompraId { get; set; }
        public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public static CarrinhoCompra GetCarrinho(IServiceProvider services)
        {
            //define uma sessão e se não for null vai invocar "HttpContext.Session" e retornar uma session;
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            //obter um serviço do tipo do nosso Contexto
            var context = services.GetService<AppDbContext>();

            //obtem ou gera o Id do Carrinho
            string carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();



            //se o valor do id for obtido eu atribuo o id do carrinho na sessão
            session.SetString("CarrinhoId", carrinhoId);

            //Retorna o carrinho com o contexto e o id atribuido ou obtido
            return new CarrinhoCompra(context)
            {
                CarrinhoCompraId = carrinhoId,
            };
        }

        /*public int BuscarIdCarrinho(int carid)
        {
            var carrinhoId = _context.CarrinhoCompraItem.Where(c => c.CarrinhoCompraId == CarrinhoCompraId).FirstOrDefault();

            if (carrinhoId != null)
            {
                return carid;
            }
            
            
        }*/

        public void AdicionarCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem = _context.CarrinhoCompraItem.SingleOrDefault(
                                      s => s.Lanche.LancheId == lanche.LancheId && //ver se existe um lanche com o id que to querendo incluir
                                      s.CarrinhoCompraId == CarrinhoCompraId); //se existe um carrinho de compra com id que eu adquiri da session ou atribui

            if (carrinhoCompraItem == null) //varificar se o item já existe no carrinho
            {
                carrinhoCompraItem = new CarrinhoCompraItem //aqui pra adicionar ele numa nova lista
                {
                    CarrinhoCompraId = CarrinhoCompraId,
                    Lanche = lanche,
                    Quantidade = 1

                };
                _context.CarrinhoCompraItem.Add(carrinhoCompraItem);
            }
            else//caso o item for diferente de null e já exisitir eu vou apenas incrementar
            {
                carrinhoCompraItem.Quantidade++;
            }

            _context.SaveChanges(); //Aqui vou salvar todas as informações no banco
        }
        public int RemoverDoCarrinho(Lanche lanche)
        {                                       //SINGLEORDEFAULT utilizo quando tivar a certeza que pode retorna "um" ou "nenhum" elemento
            var carrinhoCompraItem = _context.CarrinhoCompraItem.SingleOrDefault(
                s => s.Lanche.LancheId == lanche.LancheId && //Verificar se o lanche existe na tabela carrinhoCompraItem
                s.CarrinhoCompraId == CarrinhoCompraId); //pelo id da tabela CarrinhoCompraID pois o id é a chave da tabela

            var quantidadeLocal = 0;

            if (carrinhoCompraItem != null)
            {
                if (carrinhoCompraItem.Quantidade > 1)
                {
                    carrinhoCompraItem.Quantidade--;
                    quantidadeLocal = carrinhoCompraItem.Quantidade;
                }
                else
                {
                    _context.CarrinhoCompraItem.Remove(carrinhoCompraItem);
                }


            }

            _context.SaveChanges();
            return quantidadeLocal;
        }
        public List<CarrinhoCompraItem> GetCarrinhoCompraItens()
        {


            /*var carrinhoCompraItem = _context.CarrinhoCompraItem.Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                                                           .Include(l => l.Lanche)
                                                           .ToList();
            return carrinhoCompraItem;*/
            //se CarrinhoCompraItens for igual a null ela vai lá na tabela de CarrinhoCompra vai obter todos os carrinhos com seus itens e vai retornar uma lista com seus itens  
            return CarrinhoCompraItens ?? (CarrinhoCompraItens = _context.CarrinhoCompraItem
                .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                .Include(s => s.Lanche)
                .ToList());

        }
        public void LimparCarrinho()
        {
            //vou na lista de CarrinhoCompraItem e irá remover todos os itens de lá do carrinho
            var carrinhoItens = _context.CarrinhoCompraItem.Where(c => c.CarrinhoCompraId == CarrinhoCompraId);
            _context.CarrinhoCompraItem.RemoveRange(carrinhoItens);
            _context.SaveChanges();

        }
        public decimal GetCarrinhoCompraTotal()
        {
            var total = _context.CarrinhoCompraItem.Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                                                   .Select(l => l.Lanche.Preco * l.Quantidade).Sum(); //select ->> faz com que selecione a 
                                                                                                      //tabela junto com a
                                                                                                      //propriedade
            return total;

        }


    }
}
