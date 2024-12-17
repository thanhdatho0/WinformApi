using api.DTOs.Providerr;
using api.Models;

namespace api.Interfaces
{
    public interface IProviderRepository
    {
        Task<List<Provider>> GetAllAsync();
        Task<Provider?> GetByIdAsync(int id);
        Task<Provider> CreateAsync(Provider provider);
        Task<Provider?> UpdateAsync(int id, ProviderUpdateDto providerUpdateDto);
        Task<Provider?> DeleteAsync(int id);
        Task<bool> ProviderExists(int id);
        Task<bool> ProviderNameExists(string name);
    }
}