namespace Architecture.API.Application.Queries
{
    using Architecture.API.Application.Models;
    using Architecture.Domain.AggregatesModel;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICountryQueries
    {
        Task<CountryDto> GetByIdAsync(int id);
        Task<IEnumerable<CountryDto>> GetAll();
        
    }
}
