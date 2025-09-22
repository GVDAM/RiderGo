using RiderGo.Domain.Entities;

namespace RiderGo.Domain.Repositories
{
    public interface IRiderRepository
    {
        Task CreateAsync(Rider rider);
        Task<bool> AlreadyRegisteredAsync(string id, string cnhNumber, string cnpj);
        Task<Rider?> GetByIdAsync(string id);
        Task UpdateCnhImageAsync(string id, string cnhImageUrl);
    }
}
