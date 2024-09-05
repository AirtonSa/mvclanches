using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;

namespace LanchesMac.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context; //para acessar as tabelas do banco de dados

        public CategoriaRepository(AppDbContext context) //construtor da instancia AppDbContext
        {
            _context = context;
        }

        public IEnumerable<Categoria> Categorias => _context.Categorias;//acessar a tabela categorias e retornar a coleção de categorias na propriedade categorias
    }
}
