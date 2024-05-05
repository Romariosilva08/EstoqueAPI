using MinhaAPIEstoque.Data;
using MinhaAPIEstoque.Models;

namespace MinhaAPIEstoque.Services
{
    public class ProdutoService
    {
        private readonly MeuDbContext _dbContext;

        public ProdutoService(MeuDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InserirProduto(Produtos produto)
        {
            _dbContext.Produtos.Add(produto);
            _dbContext.SaveChanges();
        }

        public void InserirProdutos(List<Produtos> produtos)
        {
            _dbContext.Produtos.AddRange(produtos);
            _dbContext.SaveChanges();
        }
    }
}
