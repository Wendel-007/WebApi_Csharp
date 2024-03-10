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
            try {
                await _dbContext.Produtos.AddAsync(produto);
                await _dbContext.SaveChangesAsync();

                return await BuscarProdutosPorId(produto.Id);
            } catch (Exception e) {
                throw new Exception(e.InnerException.Message); //Throws exception
            }
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

            try {
                _dbContext.Produtos.Update(produtoPorId);
                await _dbContext.SaveChangesAsync(); //Throws exception
            } catch (Exception e) {
                throw new Exception(e.InnerException.Message);
            }
            return produtoPorId;
        }

        public async Task<bool> Apagar(int Id)
        {
            ProdutosModel produtoPorId = await BuscarProdutosPorId(Id);

            if (produtoPorId == null)
                return false;

            _dbContext.Produtos.Remove(produtoPorId);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
