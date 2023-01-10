using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.API.Application.Models
{
    public class Response
    {
        public List<Error> errors { get; set; }
        public double distance { get; set; }
    }
    public class Error
    {
        public string location { get; set; }
        public string param { get; set; }
        public string value { get; set; }
        public string msg { get; set; }
    }
}
