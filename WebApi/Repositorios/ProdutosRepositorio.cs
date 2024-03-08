using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Interfaces;
using WebApi.Context;

namespace WebApi.Repositorios
{
    public class ProdutosRepositorio : IProdutosRepositorio
    {
        private readonly AppDbContext _dbContext;
        public ProdutosRepositorio(AppDbContext sistemaProdutosDbContext)
        {
            _dbContext = sistemaProdutosDbContext;
        }
        public async Task<ProdutosModel> BuscarProdutosPorId(int id)
        {
            return await _dbContext.Produtos.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<ProdutosModel>> BuscarTodosOsProdutos()
        {
            return await _dbContext.Produtos.ToListAsync();
        }
        public async Task<ProdutosModel> AdicionarProdutos(ProdutosModel produto)
        {
            
            await _dbContext.Produtos.AddAsync(produto);
            try {
                await _dbContext.SaveChangesAsync();
            } catch (Exception e) {
                throw new Exception(e.InnerException.Message);
            }
            return await BuscarProdutosPorId(produto.Id);
        }

        public async Task<ProdutosModel> AtualizarProdutos(ProdutosModel produto, int Id)
        {
            ProdutosModel produtoPorId = await BuscarProdutosPorId(Id);

            if (produtoPorId == null)
                return null;

            produtoPorId.Nome = produto.Nome;
            produtoPorId.DiretorioImg = produto.DiretorioImg;
            produtoPorId.Descricao = produto.Descricao;
            produtoPorId.Categoria = produto.Categoria;
            produtoPorId.Estoque = produto.Estoque;
            produtoPorId.Preco = produto.Preco;

            _dbContext.Produtos.Update(produtoPorId);
            await _dbContext.SaveChangesAsync();

            return produtoPorId;
        }

        public async Task<bool> Apagar(int Id)
        {
            ProdutosModel produtoPorId = await BuscarProdutosPorId(Id);

            if (produtoPorId == null)
            {
                throw new Exception($"Produto de ID: {Id} não encontrado no banco de dados da aplicação!");
            }

            _dbContext.Produtos.Remove(produtoPorId);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
