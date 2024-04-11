using ms.Backend.Domain.Models;

namespace ms.Backend.Domain.IRepositories
{
    public interface IRegisterUserRepository
    {
        public Task<bool> IsValidCardAsync(string card);
        public Task<CardInformation> CardInformationAsync(string card);
        public Task<CardBalance> CardBalanceAsync(string card);
        public Task<string> RegisterUserAsync(Usuario usuario);
        public Task<List<Usuario>> GetAllUsuariosAsync();
        public Task<Usuario> GetUsuarioAsync(int Id);
        public Task<string> ModifyUserAsync(Usuario usuario);
        public Task<bool> DeleteUserAsync(int Id);
    }
}