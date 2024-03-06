using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApi.Service;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthControler : ControllerBase
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if(username == "wendelDev" && password == "1234") {
                var token = TokenService.GenerateToken(new Models.ProdutosModel());
                return Ok(token);
            }
            return BadRequest("Usuário ou Senha Inválidos!");
        }
    }
}
