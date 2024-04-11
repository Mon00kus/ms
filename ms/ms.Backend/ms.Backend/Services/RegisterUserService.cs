using ms.Backend.Domain.IRepositories;
using ms.Backend.Domain.IServices;
using ms.Backend.Domain.Models;

namespace ms.Backend.Services
{
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IRegisterUserRepository _registerUserRepository;

        public RegisterUserService(IRegisterUserRepository registerUserRepository)
        {
            _registerUserRepository = registerUserRepository;
        }

        public async Task<CardBalance> CardBalanceAsync(string card)
        {
            return await _registerUserRepository.CardBalanceAsync(card);
        }

        public async Task<CardInformation> CardInformationAsync(string card)
        {
            return await _registerUserRepository.CardInformationAsync(card);
        }

        public Task<bool> IsValidCardAsync(string card)
        {
            return _registerUserRepository.IsValidCardAsync(card);
        }

        public async Task<string> RegisterUserAsync(Usuario usuario)
        {
            return await _registerUserRepository.RegisterUserAsync(usuario);
        }

        public Task<List<Usuario>> GetAllUsuariosAsync()
        {
            return _registerUserRepository.GetAllUsuariosAsync();
        }

        public async Task<Usuario> GetUsuarioAsync(int Id)
        {
            return await _registerUserRepository.GetUsuarioAsync(Id);
        }

        public async Task<string> ModifyUserAsync(Usuario usuario)
        {
            return await _registerUserRepository.ModifyUserAsync(usuario);
        }

        public async Task<bool> DeleteUserAsync(int Id)
        {
            return await _registerUserRepository.DeleteUserAsync(Id);
        }
    }
}