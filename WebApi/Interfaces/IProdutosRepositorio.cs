using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IProdutosRepositorio
    {
        Task<List<ProdutosModel>> BuscarTodosOsProdutos();

        Task<ProdutosModel> BuscarProdutosPorId(int id);

        Task<ProdutosModel> AdicionarProdutos(ProdutosModel produto);

        Task<ProdutosModel> AtualizarProdutos(ProdutosModel produto, int Id);

        Task<bool> Apagar(int Id);


    }
}
