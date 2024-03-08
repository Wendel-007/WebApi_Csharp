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
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ProdutosController : ControllerBase
    {
        private readonly IProdutosRepositorio _produtosRepositorio;

        public ProdutosController(IProdutosRepositorio ProdutosRepositorio)
        {
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

            return Ok(new { tipo = "sucesso", mensagem = Util.Util.exitoBuscaDeProdutos, detalhes = produtos });
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)] //Nao foi encontrado nenhum produto com esse ID
        [ProducesResponseType(404)] //Requesicao incorreta
        public async Task<ActionResult<ProdutosModel>> BuscarProdutosPorId(int id)
        {

            if (id <= 0)
                return BadRequest(new { tipo = "erro", mensagem = Util.Util.idInvalido, detalhes = $"ID recebido = {id}" });

            ProdutosModel produto = await _produtosRepositorio.BuscarProdutosPorId(id);

            if (produto == null)
                return NotFound(new { tipo = "erro", mensagem = Util.Util.idNaoEncontrado, detalhes = $"ID recebido = {id}" });

            if (!ModelState.IsValid)
                return BadRequest(new { tipo = "erro", mensagem = Util.Util.erroBuscaPorId, detalhes = ModelState });

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
                    return StatusCode(500, new { tipo = "erro", mensagem = Util.Util.erroCriacao, detalhes = produto });

                return Ok(new { tipo = "sucesso", mensagem = Util.Util.exitoCriacao, detalhes = resultado });

            } catch (Exception e) {
                return BadRequest(new { tipo = "erro", mensagem  = Util.Util.erroCriacao, detalhes = e.Message });
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
                    return NotFound(new { tipo = "erro", mensagem = Util.Util.idNaoEncontrado, detalhes = $"ID recebido = {id}" });

                return Ok(new {tipo = "sucesso", mensagem = Util.Util.exitoAtualizacao, detalhes = produtoAtualizado });
            } catch (Exception e) {
                return BadRequest(new { tipo = "erro", mensagem = Util.Util.erroAtualizacao, detalhes = e.Message });
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
                return BadRequest(new { tipo = "erro", mensagem = Util.Util.idInvalido, detalhes = $"ID recebido = {id}" });

            try {

            var estadoDelecao = await _produtosRepositorio.Apagar(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(new { tipo = "erro", mensagem = Util.Util.erroDelecao, detalhes = ModelState });
            }

                if (!estadoDelecao)
                return NotFound(new { tipo = "erro", mensagem = Util.Util.idNaoEncontrado, detalhes = $"ID recebido = {id}" });
                
                return Ok(new { tipo = "sucesso", mensagem = Util.Util.exitoDelecao, detalhes = $"Produto com ID {id} removido." });

            } catch (Exception e) {
                return StatusCode(500, new { tipo = "erro", mensagem = Util.Util.erroDelecao, detalhes = e.InnerException.Message });
            }
        }
    }
}
