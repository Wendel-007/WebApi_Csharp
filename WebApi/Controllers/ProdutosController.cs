using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Context;
using WebApi.Models;
using WebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        private readonly IProdutosRepositorio _produtosRepositorio;

        public ProdutosController(AppDbContext context, IProdutosRepositorio ProdutosRepositorio)
        {
            _dbcontext = context;
            _produtosRepositorio = ProdutosRepositorio;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)] //Nao foi encontrado nenhum produto na base de dados
        public async Task<ActionResult<List<ProdutosModel>>> BuscarTodosProdutos()
        {
            List<ProdutosModel> produtos = await _produtosRepositorio.BuscarTodosOsProdutos();

            if (!produtos.Any())
                return NoContent();

            return Ok(produtos);
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)] //Nao foi encontrado nenhum produto com esse ID
        [ProducesResponseType(404)] //Requesicao incorreta
        public async Task<ActionResult<ProdutosModel>> BuscarProdutosPorId(int id)
        {

            ProdutosModel produto = await _produtosRepositorio.BuscarProdutosPorId(id);

            if (produto == null)
                return NotFound($"Produto de ID: {id} não encontrado no banco de dados da aplicação.");

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(produto);
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
                    return StatusCode(500, "Um ERRO ocorreu a criacao desse produto.");

                return Ok(resultado);

            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPut("atualizar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProdutosModel>> AtualizarProdutos(ProdutosModel produto, int id)
        {
            var produtoAtualizado = await _produtosRepositorio.AtualizarProdutos(produto, id);

            if(produtoAtualizado == null)
                return NotFound($"Produto de ID: {id} não encontrado no banco de dados da aplicação.");
            
            return Ok(produtoAtualizado);
                
            /*ProdutosModel? produtoPorId = await _dbcontext.Produtos.FindAsync(id);

            if (produtoPorId == null)
            {
               return NotFound($"Produto de ID: {id} não encontrado no banco de dados da aplicação.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            produtoPorId.Nome = produto.Nome;
            produtoPorId.DiretorioImg = produto.DiretorioImg;
            produtoPorId.Descricao = produto.Descricao;
            produtoPorId.Categoria = produto.Categoria;
            produtoPorId.Estoque = produto.Estoque;
            produtoPorId.Preco = produto.Preco;

            _dbcontext.Produtos.Update(produtoPorId);
            await _dbcontext.SaveChangesAsync();

            return Ok(produtoPorId);*/
        }

        [Authorize]
        [HttpDelete("deletar")]
        [ProducesResponseType(204)] //Delecao feita com sucesso
        [ProducesResponseType(404)] //Item a ser deletado nao existe
        [ProducesResponseType(500)] //Erro na API durante a delecao
        public async Task<ActionResult<bool>> Apagar(int id)
        {
            ProdutosModel produtoPorId = await _dbcontext.Produtos.FindAsync(id);

            if (produtoPorId == null)
                return NotFound($"Produto de ID: {id} não encontrado no banco de dados da aplicação.");
            

            _dbcontext.Produtos.Remove(produtoPorId);
            if (await _dbcontext.SaveChangesAsync() <= 0)
                return StatusCode(500, "Um ERRO ocorreu durante a delecao desse produto.");

            return Ok($"Delecao do produto com ID {id} realizada com sucesso\n {produtoPorId}");
        }
    }
}
