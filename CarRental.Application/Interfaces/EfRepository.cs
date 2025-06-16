using CarRental.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using CarRental.Infrastructure.Persistence;

namespace CarRental.Infrastructure.Persistence
{
    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly CarRentalDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EfRepository(CarRentalDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? Get(int id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
