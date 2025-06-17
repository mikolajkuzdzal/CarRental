using CarRental.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using CarRental.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRental.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly CarRentalDbContext _ctx;
        public EfRepository(CarRentalDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _ctx.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(int id) =>
            await _ctx.Set<T>().FindAsync(id);

        public async Task AddAsync(T entity)
        {
            await _ctx.Set<T>().AddAsync(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _ctx.Set<T>().Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}
