using Architecture.Domain.AggregatesModel;
using Architecture.Domain.SeedWork;
using System.Threading.Tasks;

namespace Architecture.Domain.AggregatesModel
{
    public interface ICountryRepository : IRepository<Country>
    {
        Country Add(Country entity);

        Country Update(Country entity);

        void Delete(Country entity);

        Task<Country> FindByIdAsync(int Id);

        //DDD: No GetList() method as it will be available at the QUERY level.

    }
}
