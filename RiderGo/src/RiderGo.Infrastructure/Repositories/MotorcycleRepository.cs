using Microsoft.EntityFrameworkCore;
using RiderGo.Domain.Repositories;
using RiderGo.Infrastructure.Data;
using RiderGo.Domain.Entities;

namespace RiderGo.Infrastructure.Repositories
{
    public class MotorcycleRepository(AppDbContext context) : IMotorcycleRepository
    {
        private readonly AppDbContext _context = context;

        public async Task CreateAsync(Motorcycle motorcycle)
        {
            await _context.Motorcycles.AddAsync(motorcycle);
            await _context.SaveChangesAsync();
        }        

        public async Task UpdatePlateAsync(string id, string plate)
        {
            await _context.Motorcycles
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => 
                    x.SetProperty(m => m.Plate, plate));

            await _context.SaveChangesAsync();
        }

        public async Task<Motorcycle?> GetByIdAsync(string id)
        {
            var result = await _context.Motorcycles
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.Id == id);

            return result;

        }

        public async Task<IEnumerable<Motorcycle>> GetAll(string? plate)
        {
            var query = _context.Motorcycles
                            .AsNoTracking()
                            .AsQueryable();

            if (!string.IsNullOrWhiteSpace(plate))
                query = query.Where(x => x.Plate.Contains(plate));


            return await query.ToListAsync();
        }

        public async Task DeleteAsync(Motorcycle motorcycle)
        {
            _context.Remove(motorcycle);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AlreadyRegisteredAsync(string plata, string id)
        {
            return await _context.Motorcycles
                        .AsNoTracking()
                        .AnyAsync(x => x.Plate == plata || x.Id == id);
        }

        public async Task<bool> HasAlreadyBeenRentAsync(string motorcycleId)
        {
           return await _context.Rentals
                        .AsNoTracking()
                        .AnyAsync(x => x.MotorcycleId == motorcycleId);
        }

        public async Task UpdateAsync(Motorcycle motorcycle)
        {
            _context.Entry(motorcycle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
