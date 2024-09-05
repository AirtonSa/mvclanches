using LanchesMac.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Components
{
    public class CategoriaMenu : ViewComponent
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaMenu(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public IViewComponentResult Invoke()
        {

         var categorias = _categoriaRepository.Categorias.OrderBy(c => c.CategoriaNome);//vai lá na base de dados e trazer as categorias

            return View(categorias);
        }
    }
}
