using RiderGo.Domain.Entities;

namespace RiderGo.Domain.Repositories
{
    public interface IMotorcycleRepository
    {
        Task CreateAsync(Motorcycle motorcycle);
        Task UpdatePlateAsync(string id, string plate);
        Task<Motorcycle?> GetByIdAsync(string id);
        Task<IEnumerable<Motorcycle>> GetAll(string? plate);
        Task DeleteAsync(Motorcycle motorcycle);
        Task<bool> AlreadyRegisteredAsync(string plate, string id);
        Task<bool> HasAlreadyBeenRentAsync(string motorcycleId);
        Task UpdateAsync(Motorcycle motorcycle);
    }
}
