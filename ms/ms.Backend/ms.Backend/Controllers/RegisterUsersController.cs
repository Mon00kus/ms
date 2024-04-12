using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms.Backend.Domain.IServices;
using ms.Backend.Domain.Models;

namespace ms.Backend.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RegisterUsersController : ControllerBase
    {
        private readonly IRegisterUserService _registerUserService;

        public RegisterUsersController(IRegisterUserService registerUserService)
        {
            _registerUserService = registerUserService;
        }

        [HttpGet("IsValidCard/{card}")]
        public async Task<IActionResult> IsValidCard(string card)
        {
            var responseTuLlave = await _registerUserService.IsValidCardAsync(card);
            if (responseTuLlave == true)
            {
                return Ok(new { message = "Tarjeta valida" });
            }
            return BadRequest("Tarjeta No valida");
        }

        [HttpGet("getCardInformation/{card}")]
        public async Task<IActionResult> getCardInformation(string card)
        {
            var responseTuLlave = await _registerUserService.CardInformationAsync(card);

            return Ok(responseTuLlave);
        }

        [HttpGet("getCardBalance/{card}")]
        public async Task<IActionResult> getCardBalance(string card)
        {
            var responseTuLlave = await _registerUserService.CardBalanceAsync(card);

            return Ok(responseTuLlave);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _registerUserService.GetAllUsuariosAsync();

            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAsync(int Id)
        {
            var result = await _registerUserService.GetUsuarioAsync(Id);

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Usuario usuario)
        {
            string resultMessage = "";

            if (
                string.IsNullOrEmpty(usuario.Serial_tullave) ||
                string.IsNullOrEmpty(usuario.Firtsname) ||
                string.IsNullOrEmpty(usuario.Lastname) ||
                string.IsNullOrEmpty(usuario.DocumentType) ||
                string.IsNullOrEmpty(usuario.Document) 
                //|| usuario.Saldo == 0
               )
            {
                return BadRequest("Debe completar la información de todos los campos !!!");
            }

            var isValidTuLlave = await _registerUserService.IsValidCardAsync(usuario.Serial_tullave);
            var cardbalance = await _registerUserService.CardBalanceAsync(usuario.Serial_tullave);
            if (isValidTuLlave)
            {
                usuario.Saldo = cardbalance.balance;
                resultMessage = await _registerUserService.RegisterUserAsync(usuario);
            }
            else
            {
                return BadRequest("No se pudo registrar el usuario con esta tarjeta - Serial invalido");
            }

            if (resultMessage.Contains("Error al registrar usuario"))
            {
                if (resultMessage.Contains("duplicate key value"))
                {
                    return BadRequest("Violación de valor unico en Constraint");
                }
                return BadRequest(resultMessage);
            }
            
            var cardInformation = await _registerUserService.CardInformationAsync(usuario.Serial_tullave);
            var nombreCompleto =
                string.IsNullOrEmpty(cardInformation.userName) ? "Sin Nombre " : cardInformation.userName;
            var apellidoCompleto =
                string.IsNullOrEmpty(cardInformation.userLastName) ? "Sin Apellido" : cardInformation.userLastName;

            return Ok(new
            {
                message = ("Usuario : " + usuario.Firtsname + " " + usuario.Lastname + " registrado con exito en PostGres").ToUpper(),
                usuarioEnCard = nombreCompleto + " " + apellidoCompleto,
                bancoEnCard = cardInformation.bankName == null ? "Sin banco" : cardInformation.bankName,
                saldoEnCard = cardbalance.balance,
                saldoVirtualEnCard = cardbalance.virtualBalance
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public async Task<IActionResult> PutAsync(Usuario usuario)
        {
            var resultMessage = await _registerUserService.ModifyUserAsync(usuario);
            if (resultMessage.Contains("Error al actualizar usuario"))
            {
                return BadRequest(resultMessage);
            }
            return Ok(resultMessage);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            var result = await _registerUserService?.DeleteUserAsync(Id)!;
            return Ok(result);
        }
    }
}