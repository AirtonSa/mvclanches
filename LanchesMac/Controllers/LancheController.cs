using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class LancheController : Controller
    {
        private readonly ILancheRepository _lancheRepository; //acessar o banco pela interface

        public LancheController(ILancheRepository lancheRepository) //isso é apenas uma instancia so repository

        {
            _lancheRepository = lancheRepository;
        }
        public IActionResult List(string categoria) //controller que vai passar para a view as listas de lanche que estão no banco de dados
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if (categoria == null) // ou (string.IsNullOrEmpty(categoria)) //Verificar se a categoria é vazia ou nula
            {
                lanches = _lancheRepository.Lanches.OrderBy(l => l.LancheId); //aqui estou ordenando todos os lanches pelo id
                categoriaAtual = "Todos os lanches";
            }
            else
            {
                /*if (string.Equals("Normal", categoria, StringComparison.OrdinalIgnoreCase)) //aqui estou veificando se a minha string é igual(Equals) ao "normal"//considerar caixa alta e caixa baixa
                {
                    lanches = _lancheRepository.Lanches
                        .Where(c => c.Categoria.CategoriaNome.Equals("Normal"))
                        .OrderBy(l => l.Nome);
                }
                else
                {
                    lanches = _lancheRepository.Lanches
                        .Where(c => c.Categoria.CategoriaNome.Equals("Natural"))
                        .OrderBy(c => c.Nome);
                }*/

                lanches = _lancheRepository.Lanches //melhor assim pq dar para fazer alteração no banco de dados para acrescentar categoria
                    .Where(l => l.Categoria.CategoriaNome
                    .Equals(categoria))
                    .OrderBy(c => c.Nome);
            }

            var lanchesListViewModel = new LancheListViewModels
            {
                Lanche = lanches,
                CategoriaAtual = categoriaAtual

            };

            return View(lanchesListViewModel);


        }

        public IActionResult Details(int LancheId)
        {
            var lanche = _lancheRepository.Lanches.FirstOrDefault(l => l.LancheId == LancheId);

            return View(lanche);
        }

        public ViewResult Search (string searchString)
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = "";

            if (searchString == null)
            {
                lanches = _lancheRepository.Lanches.OrderBy(p => p.LancheId);
                categoriaAtual = "Todos os Lanches";
            }
            else
            {
                lanches = _lancheRepository.Lanches
                    .Where(p => p.Nome.ToLower().Contains(searchString.ToLower()));
                //TOLOWER faz com que a pesquisa seja com letra maiuscula ou minuscula
                //CONTAINS faz uma comparação de igualdade para saber se o elemento existe na lista

                if (lanches.Any())
                {
                    //ANY verifica se acoleção tem o seguinte elemento ex: "Nessa coleção(lista) tem o elemento com o nome Misto?'
                    categoriaAtual = "Lanches";

                }
                else
                {
                    categoriaAtual = "Nenhum lanche foi encontrado";
                }
            }

            return View("~/Views/Lanche/List.cshtml", new LancheListViewModels
            {
                Lanche = lanches,
                CategoriaAtual = categoriaAtual

            });
                
        }
    }
}
