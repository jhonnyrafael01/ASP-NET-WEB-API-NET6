using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")] // /produtos
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos()
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        // /api/produtos
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            //var produtos = _uof.Produtos.Take(10).ToList(); limita a 10 produtos total
            var produtos = _uof.ProdutoRepository.Get().ToList();

            if (produtos is null)
            {
                return NotFound("Produtos não encontrados...");
            }
            return produtos;
        }

        // /api/produtos/id
        [HttpGet("{id:int:min(1)}/{nome=Caderno}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id, string nome)
        {
            var parametro = nome;

            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return produto;
        }

        // /api/produtos
        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest();

            _uof.ProdutoRepository.Add(produto);
            _uof.Commit();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest();
            }

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
            
            if (produto is null)
            {
                return NotFound("Produto não localizado...");
            }
            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit(); 

            return Ok(produto); 
        }
    }
}
