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

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        
        public ProdutosController(AppDbContext context)
        {
            _dbcontext = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<ProdutosModel>>> BuscarTodosProdutos()
        {
            List<ProdutosModel> produtos = await _dbcontext.Produtos.ToListAsync();
            return Ok(produtos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutosModel>> BuscarProdutosPorId(int id)
        {
            ProdutosModel produto = await _dbcontext.Produtos.FindAsync(id);
            return Ok(produto);
        }

        [Authorize]
        [HttpPost("cadastrar")]
        public async Task<ProdutosModel> AdicionarProdutos(ProdutosModel produto)
        {
            await _dbcontext.Produtos.AddAsync(produto);
            await _dbcontext.SaveChangesAsync();
            return produto;
        }

        [Authorize]
        [HttpPut("atualizar")]
        public async Task<ProdutosModel> AtualizarProdutos(ProdutosModel produto, int Id)
        {
            ProdutosModel produtoPorId = await _dbcontext.Produtos.FindAsync(Id);

            if (produtoPorId == null)
            {
                throw new Exception($"Produto de ID: {Id} não encontrado no banco de dados da aplicação!");
            }

            produtoPorId.Nome = produto.Nome;
            produtoPorId.DiretorioImg = produto.DiretorioImg;
            produtoPorId.Descricao = produto.Descricao;
            produtoPorId.Categoria = produto.Categoria;
            produtoPorId.Estoque = produto.Estoque;
            produtoPorId.Preco = produto.Preco;

            _dbcontext.Produtos.Update(produtoPorId);
            await _dbcontext.SaveChangesAsync();

            return produtoPorId;
        }

        [Authorize]
        [HttpDelete("deletar")]
        public async Task<bool> Apagar(int Id)
        {
            ProdutosModel produtoPorId = await _dbcontext.Produtos.FindAsync(Id);

            if (produtoPorId == null)
            {
                throw new Exception($"Produto de ID: {Id} não encontrado no banco de dados da aplicação!");
            }

            _dbcontext.Produtos.Remove(produtoPorId);
            await _dbcontext.SaveChangesAsync();

            return true;
        }
    }
}
