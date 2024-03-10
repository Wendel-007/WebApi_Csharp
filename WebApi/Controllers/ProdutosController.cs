using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;


namespace WebApi.Controllers
{

    [ApiController]
    [Route("Produtos")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosRepositorio _produtosRepositorio;

        public ProdutosController(IProdutosRepositorio ProdutosRepositorio)
        {
            _produtosRepositorio = ProdutosRepositorio;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)] //Nao foi encontrado nenhum produto na base de dados
        public async Task<ActionResult<List<ProdutosModel>>> BuscarTodosProdutos()
        {
            List<ProdutosModel> produtos = await _produtosRepositorio.BuscarTodosOsProdutos();

            if (!produtos.Any())
                return NoContent();

            return Ok(Util.Util.msgRetorno(200, Util.Util.exitoBuscaDeProdutos, produtos));
            //new { tipo = "sucesso", mensagem = Util.Util.exitoBuscaDeProdutos, detalhes = produtos }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)] //Nao foi encontrado nenhum produto com esse ID
        [ProducesResponseType(404)] //Requesicao incorreta
        public async Task<ActionResult<ProdutosModel>> BuscarProdutosPorId(int id)
        {

            if (id <= 0)
                return BadRequest(Util.Util.msgRetorno(400, Util.Util.idInvalido, $"ID recebido = {id}"));
            //new { tipo = "erro", mensagem = Util.Util.idInvalido, detalhes = $"ID recebido = {id}" }
            ProdutosModel produto = await _produtosRepositorio.BuscarProdutosPorId(id);

            if (produto == null)
                return NotFound(Util.Util.msgRetorno(404, Util.Util.idNaoEncontrado, $"ID recebido = {id}"));
            //new { tipo = "erro", mensagem = Util.Util.idNaoEncontrado, detalhes = $"ID recebido = {id}" }
            if (!ModelState.IsValid)
                return BadRequest(Util.Util.msgRetorno(400, Util.Util.erroBuscaPorId, ModelState));
            //new { tipo = "erro", mensagem = Util.Util.erroBuscaPorId, detalhes = ModelState }
            return Ok(new { tipo = "sucesso", mensagem = Util.Util.exitoBuscaPorId, detalhes = produto });
        }

        [Authorize]
        [HttpPost("cadastrar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProdutosModel>> AdicionarProdutos(ProdutosModel produto)
        {
            try {
                var resultado = await _produtosRepositorio.AdicionarProdutos(produto);

                if (resultado == null)
                    return StatusCode(500, Util.Util.msgRetorno(500, Util.Util.erroCriacao, produto));
                //new { tipo = "erro", mensagem = Util.Util.erroCriacao, detalhes = produto }
                return Ok(Util.Util.msgRetorno(200, Util.Util.exitoCriacao, resultado));
                //new { tipo = "sucesso", mensagem = Util.Util.exitoCriacao, detalhes = resultado }
            }
            catch (Exception e) {
                return BadRequest(Util.Util.msgRetorno(400, Util.Util.erroCriacao, e.Message));
                //new { tipo = "erro", mensagem  = Util.Util.erroCriacao, detalhes = e.Message }
            }
        }

        [Authorize]
        [HttpPut("atualizar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProdutosModel>> AtualizarProdutos(ProdutosModel produto, int id)
        {
            try {
                var produtoAtualizado = await _produtosRepositorio.AtualizarProdutos(produto, id);

                if (produtoAtualizado == null)
                    return NotFound(Util.Util.msgRetorno(404, Util.Util.idNaoEncontrado, $"ID recebido = {id}"));
                //new { tipo = "erro", mensagem = Util.Util.idNaoEncontrado, detalhes = $"ID recebido = {id}" }

                return Ok(Util.Util.msgRetorno(200, Util.Util.exitoAtualizacao, produtoAtualizado));
                //new {tipo = "sucesso", mensagem = Util.Util.exitoAtualizacao, detalhes = produtoAtualizado }

            }
            catch (Exception e) {
                return BadRequest(Util.Util.msgRetorno(400, Util.Util.erroAtualizacao, e.Message));
                //
            
            }
        }

        [Authorize]
        [HttpDelete("deletar")]
        [ProducesResponseType(204)] //Delecao feita com sucesso
        [ProducesResponseType(404)] //Item a ser deletado nao existe
        [ProducesResponseType(500)] //Erro na API durante a delecao
        public async Task<ActionResult<bool>> Apagar(int id)
        {
            if (id <= 0)
                return BadRequest(Util.Util.msgRetorno(400, Util.Util.idInvalido, $"ID recebido = {id}"));

            try {

            var estadoDelecao = await _produtosRepositorio.Apagar(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(Util.Util.msgRetorno(400, Util.Util.erroDelecao, ModelState));
            }

                if (!estadoDelecao)
                return NotFound(Util.Util.msgRetorno(404, Util.Util.idNaoEncontrado, $"ID recebido = {id}"));
                //new { tipo = "erro", mensagem = Util.Util.idNaoEncontrado, detalhes = $"ID recebido = {id}" }
                return Ok(Util.Util.msgRetorno(200, Util.Util.exitoDelecao, $"Produto com ID {id} removido."));
                //new { tipo = "sucesso", mensagem = Util.Util.exitoDelecao, detalhes = $"Produto com ID {id} removido." }
            }
            catch (Exception e) {
                return StatusCode(500, Util.Util.msgRetorno(500, Util.Util.erroDelecao, e.InnerException.Message));
                //new { tipo = "erro", mensagem = Util.Util.erroDelecao, detalhes = e.InnerException.Message }
            }
        }
    }
}
