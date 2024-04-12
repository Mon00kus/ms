using Microsoft.EntityFrameworkCore;
using ms.Backend.Data;
using ms.Backend.Domain.IRepositories;
using ms.Backend.Domain.Models;
using ms.Backend.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ms.Backend.Persistence.Repositories
{
    public class RegisterUserRepository : IRegisterUserRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<RegisterUserService> _logger;        
        private readonly IConfiguration _configuration;

        public RegisterUserRepository(DataContext context,
                                   ILogger<RegisterUserService> logger,                                   
                                   IConfiguration configuration)
        {
            _context = context;
            _logger = logger;            
            _configuration = configuration;
        }

        public async Task<CardBalance> CardBalanceAsync(string card)
        {
            var tokenTuLlave = _configuration["JwtBearer:TokenTullave"];
            var endPointTuLlave = _configuration["EndsTuLlave:CardBalance"];
            CardBalance CardBalanceResponse = new CardBalance();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", tokenTuLlave);

                var apiUrl = $"{endPointTuLlave}{card}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                string responseContent = await response.Content.ReadAsStringAsync();

                CardBalanceResponse = JsonConvert.DeserializeObject<CardBalance>(responseContent)!;
            }
            return CardBalanceResponse;
        }

        public async Task<CardInformation> CardInformationAsync(string card)
        {
            var tokenTuLlave = _configuration["JwtBearer:TokenTullave"];
            var endPointTuLlave = _configuration["EndsTuLlave:CardInformation"];
            CardInformation CardInformationResponse = new CardInformation();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", tokenTuLlave);

                var apiUrl = $"{endPointTuLlave}{card}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                string responseContent = await response.Content.ReadAsStringAsync();

                CardInformationResponse= JsonConvert.DeserializeObject<CardInformation>(responseContent)!;
            }
            return CardInformationResponse;
        }
 
        public async Task<bool> IsValidCardAsync(string card)
        {      
            var tokenTuLlave = _configuration["JwtBearer:TokenTullave"];
            var endPointTuLlave = _configuration["EndsTuLlave:ValidCard"];
            
            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", tokenTuLlave);
                
                var apiUrl = $"{endPointTuLlave}{card}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                string responseContent = await response.Content.ReadAsStringAsync();

                ValidCard validationResponse = JsonConvert.DeserializeObject<ValidCard>(responseContent)!;

                if (validationResponse.isValid) 
                {
                    return true;
                }                                
            }
            return false;
        }

        public async Task<string> RegisterUserAsync(Usuario usuario)
        {
            try
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al registrar usuario: " + ex.Message);
                return "Error al registrar usuario - " + ex.Message + " - " + ex.InnerException ;
            }
            return "Registro de usuario exitoso";
        }

        public async Task<List<Usuario>> GetAllUsuariosAsync()
        {
            var listing = await _context.Usuarios.ToListAsync();

            return listing;
        }

        public async Task<Usuario> GetUsuarioAsync(int Id)
        {
            Usuario? user = await _context.Usuarios.FindAsync(Id);
            return user!;
        }

        public async Task<string> ModifyUserAsync(Usuario usuario)
        {
            try
            {
                var existingUser = await _context.Usuarios.FindAsync(usuario.Id);
                if (existingUser == null)
                {
                    return "Usuario no encontrado";
                }
                
                _context.Entry(existingUser).CurrentValues.SetValues(usuario);

                _context.Entry(existingUser).Property(u => u.Serial_tullave).IsModified = false;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al actualizar usuario: " + ex.Message);
                return "Error al actualizar usuario - " + ex.Message + " - " + ex.InnerException;
            }
            return "Modificación de usuario exitosa";
        }

        public async Task<bool> DeleteUserAsync(int Id)
        {
            try
            {
                var existingUser = await _context.Usuarios.FindAsync(Id);
                if (existingUser == null)
                {
                    return false;
                }
                else
                {
                   _context.Usuarios.Remove(existingUser);
                   await _context.SaveChangesAsync();
                   return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al actualizar usuario: " + ex.Message);
                return false;
            }            
        }
    }
}