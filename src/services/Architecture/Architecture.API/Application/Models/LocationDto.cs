using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.API.Application.Models
{
    public class LocationDto
    {
        public string city { get; set; }
        public string city_iata { get; set; }
        public string country { get; set; }
        public string country_iata { get; set; }
        public int hubs { get; set; }
        public string iata { get; set; }
        public CoordinateDto location { get; set; }
        public string name { get; set; }
        public int rating { get; set; }
        public string timezone_region_name { get; set; }
        public string type { get; set; }
    }
}
