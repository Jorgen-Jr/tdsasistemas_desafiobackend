using DesafioBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DesafioBackend.Controllers
{
    [Route("getRandomToken")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _config;

        public AuthenticationController(IConfiguration config)
        {
            _config = config;
        }

        /*  Gerar e retornar uma string com o token de acesso.
         *  Seria o resultado da rota de login.
         */
        [HttpGet]
        public string GetRandomToken()
        {
            var jwt = new JwtService(_config);
            var token = jwt.GenerateSecurityToken("someUser@aspdok.com");
            return token;
        }
    }
}
