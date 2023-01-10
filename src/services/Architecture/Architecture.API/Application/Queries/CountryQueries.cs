using Architecture.API.Application.Models;
using Architecture.Domain.AggregatesModel;
using Architecture.Domain.Extensions;
using Architecture.Infrastructure.Helpers;
using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace Architecture.API.Application.Queries
{
    public class CountryQueries : ICountryQueries
    {
        private readonly string _connectionString;

        public CountryQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<IEnumerable<CountryDto>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var query = $@"Your Query";
                    var result = await connection.QueryAsync<CountryDto>(query);

                    return result;
                }
                catch (Exception ex)
                {
                    Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                    throw;
                }
            }
        }

        public async Task<CountryDto> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    var query = $@"Your Query";
                    var result = await connection.QueryFirstOrDefaultAsync<CountryDto>(query, param: new { Id = id });

                    return result;
                }
                catch (Exception ex)
                {
                    Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                    throw;
                }
            }
        }
    }
}
