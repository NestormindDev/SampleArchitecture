using Architecture.API.Application.Models;
using Architecture.API.Application.Queries;
using Architecture.Domain.Extensions;
using Dapper;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading.Tasks;


namespace Architecture.API.Application.Queries
{
    public class LocationQueries : ILocationQueries
    {
        static HttpClient client = new HttpClient();
        private readonly string _connectionString;

        public LocationQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<Response> GetDistanceAsync(string location_one, string location_two)
        {
            Response response = new Response();
            try
            {
                var loc_1 = await client.GetAsync("http://pinotto.com/airports/" + location_one);
                var loc_2 = await client.GetAsync("http://pinotto.com/airports/" + location_two);
                var res1 = await loc_1.Content.ReadAsStringAsync();
                var res2 = await loc_2.Content.ReadAsStringAsync();

                if (!res1.Contains("errors") && !res2.Contains("errors"))
                {
                    var location_1 = JsonConvert.DeserializeObject<LocationDto>(res1);
                    var location_2 = JsonConvert.DeserializeObject<LocationDto>(res2);
                    response.distance = CalculateDistance(location_1, location_2);
                }
                else
                {
                    if (res1.Contains("errors"))
                        response = JsonConvert.DeserializeObject<Response>(res1);
                    else if (res2.Contains("errors"))
                        response= JsonConvert.DeserializeObject<Response>(res2);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                response.errors = new List<Error> { new Error { msg = $"Exception: {JsonConvert.SerializeObject(ex)}" } };
            }
            return response;
        }

        private double CalculateDistance(LocationDto point1, LocationDto point2)
        {
            var d1 = point1.location.lat * (Math.PI / 180.0);
            var num1 = point1.location.lon * (Math.PI / 180.0);
            var d2 = point2.location.lat * (Math.PI / 180.0);
            var num2 = point2.location.lon * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
