using Microsoft.EntityFrameworkCore;
using RiderGo.Infrastructure.Data;
using RiderGo.Domain.Repositories;
using RiderGo.Domain.Entities;

namespace RiderGo.Infrastructure.Repositories
{
    public class RentalRepository(AppDbContext context) : IRentalRepository
    {
        private readonly AppDbContext _context = context;
               

        public async Task CreateRentalAsync(Rental rental)
        {
            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
        }

        public async Task<Rental?> GetRentalByIdAsync(Guid id)
        {
            return await _context.Rentals
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> HasRiderValidCnhTypeAsync(string riderId)
        {
            return await _context.Riders
                .AsNoTracking()
                .AnyAsync(x => x.Id == riderId && (x.CnhType.Contains("A")) );
        }

        public async Task<bool> AlreadyExistRentAsync(string riderId, string motorcycleId)
        {
            return await _context.Rentals
                .AsNoTracking()
                .AnyAsync(x => x.RiderId == riderId && x.MotorcycleId == motorcycleId && x.ReturnDate > DateTime.UtcNow);
        }

        public async Task<bool> IsMotocycleAvaibleToRentAsync(string motorcycleId)
        {
            return await _context.Motorcycles
                .AsNoTracking()
                .AnyAsync(x => x.Id == motorcycleId);
        }

        public async Task<bool> IsMotorcycleCurrentlyRentedAsync(string motorcycleId)
        {
            return await _context.Rentals
                .AsNoTracking()
                .AnyAsync(x => x.MotorcycleId == motorcycleId && x.ReturnDate == null);
        }

        public async Task UpdateRentalAsync(Rental rental)
        {
            _context.Entry(rental).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
