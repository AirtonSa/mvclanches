using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Repositories
{
    public class LanchesRepository : ILancheRepository
    {
        private readonly AppDbContext _context; //para acessar as tabelas do banco de dados

        public LanchesRepository(AppDbContext context) //construtor da instancia AppDbContext
        {
            _context = context;
        }


        public IEnumerable<Lanche> Lanches => _context.Lanches.Include(c => c.Categoria); //incluir categorias no lanches obtidos

        public IEnumerable<Lanche> LanchesPreferidos => _context.Lanches
                                                        .Where(l => l.IsLanchePreferido)
                                                        .Include(c => c.Categoria);

        public Lanche GetLancheById(int lancheId)
        {
            return _context.Lanches.FirstOrDefault(l => l.LancheId == lancheId);
        }
    }
}
