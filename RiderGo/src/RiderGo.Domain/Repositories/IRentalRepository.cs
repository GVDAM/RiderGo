using RiderGo.Domain.Entities;

namespace RiderGo.Domain.Repositories
{
    public interface IRentalRepository
    {
        Task<bool> AlreadyExistRentAsync(string riderId, string motorcycleId);
        Task CreateRentalAsync(Rental rental);
        Task<Rental?> GetRentalByIdAsync(Guid id);
        Task<bool> HasRiderValidCnhTypeAsync(string riderId);
        Task<bool> IsMotocycleAvaibleToRentAsync(string motorcycleId);
        Task UpdateRentalAsync(Rental rental);
        Task<bool> IsMotorcycleCurrentlyRentedAsync(string motorcycleId);
    }
}
