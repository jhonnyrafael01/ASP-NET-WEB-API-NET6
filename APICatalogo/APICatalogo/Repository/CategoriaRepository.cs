using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters)
        {
            return PagedList<Categoria>.ToPagedeList(Get().OrderBy(on => on.Nome), categoriasParameters.PageNumber, categoriasParameters.PageSize);
        }

        public IEnumerable<Categoria> GetCategoriaProdutos()
        {
            return Get().Include(x => x.Produtos);
        }        
    }
}
