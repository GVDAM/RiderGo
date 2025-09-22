using Microsoft.EntityFrameworkCore;
using RiderGo.Domain.Repositories;
using RiderGo.Infrastructure.Data;
using RiderGo.Domain.Entities;

namespace RiderGo.Infrastructure.Repositories
{
    public class RiderRepository(AppDbContext context) : IRiderRepository
    {
        private readonly AppDbContext _context = context;

        public async Task CreateAsync(Rider rider)
        {
            await _context.Riders.AddAsync(rider);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AlreadyRegisteredAsync(string id, string cnhNumber, string cnpj)
        {
            return await _context.Riders
                 .AsNoTracking()
                 .AnyAsync(r => r.CnhNumber == cnhNumber || r.CNPJ == cnpj || r.Id == id);
        }

        public async Task<Rider?> GetByIdAsync(string id)
        {
            return await _context.Riders
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateCnhImageAsync(string id, string cnhImageUrl)
        {
            await _context.Riders
                .Where(r => r.Id == id)
                .ExecuteUpdateAsync(r => r.SetProperty(r => r.CnhImageUrl, cnhImageUrl));
        }
    }
}
