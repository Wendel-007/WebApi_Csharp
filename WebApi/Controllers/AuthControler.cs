using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Service;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthControler : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> auth([FromBody] AuthModel ReqBody)
        {
            /*
             * Essa checagem condicional abaixo nunca é executada, 
             *  pois quando o body JSON da requisição está errado, 
             *   alguma outra parte do codigo dispara um erro antes de executar esta função.
            */
            if(ReqBody.usuario == null || ReqBody.senha == null)
            {
                return BadRequest(Util.Util.msgRetorno(400, Util.Util.erroAutenticacao, ReqBody.ToString()));
            }

            if(ReqBody.usuario == "wendelDev" && ReqBody.senha == "1234") {
                var token = TokenService.GenerateToken(new Models.ProdutosModel());
                string conteudoToken = token.ToString().Replace("{ token =", "").Replace("}", "").Trim();
                return BadRequest(Util.Util.msgRetorno(200, Util.Util.exitoAutenticacao, conteudoToken));
            }
            return Unauthorized(Util.Util.msgRetorno(401, Util.Util.invalidaAutenticacao, "-"));
        }
    }
}
