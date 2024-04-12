using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ms.Backend.DTO;
using ms.Backend.Helpers;

namespace ms.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpPost("getToken")]
        public IActionResult Login([FromBody] UserLogin user)
        {            
            try
            {
                // Llega la info del user por parametro, no se valida absoultamente nada porque es info statica
                user.Id = 1;

                var token = JwtConfigurator.GetToken(user, _config);

                return Ok(new { Token = token });

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }            
        }
    }
}