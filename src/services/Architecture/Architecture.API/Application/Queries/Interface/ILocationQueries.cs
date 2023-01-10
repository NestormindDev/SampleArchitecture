using Architecture.API.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.API.Application.Queries
{
    public interface ILocationQueries
    {
        Task<Response> GetDistanceAsync(string location_one, string location_two);
    }
}
