using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ms.Backend.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RegisterUserController : ControllerBase
    {
        private readonly ILogger<RegisterUserController> _logger;
        public RegisterUserController(ILogger<RegisterUserController> logger)
        {

            _logger = logger;

        }
        [HttpGet("IsValidCard/{card}")]        
        public async Task<IActionResult> IsValidCard(string card)
        {
            try
            {                
                return Ok("Serial tarjeta" )!;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}