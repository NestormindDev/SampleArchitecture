using Architecture.Domain.AggregatesModel;
using Architecture.Domain.SeedWork;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Architecture.Infrastruture.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ArchitectureContext _context;
        public IUnitOfWork UnitOfWork
        {
            get { return _context; }
        }

        public CountryRepository(ArchitectureContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Country Add(Country item)
        {
            if (item.IsTransient())
            {
                return _context.Countries.Add(item).Entity;
            }
            else
            {
                return item;
            }
        }

        public async Task<Country> FindByIdAsync(int Id)
        {
            var item = await _context.Countries.Where(r => r.Id == Id).FirstOrDefaultAsync();

            return item;
        }

        public Country Update(Country item)
        {
            return _context.Countries
                .Update(item)
                .Entity;
        }

        public void Delete(Country entity)
        {
            _context.Countries.Remove(entity);
        }
    }
}
