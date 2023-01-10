using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.API.Application.Models
{
    public class User
    {
        public int EmployeeId { get; set; }
        public string UserGuid { get; set; }
        public string Email { get; set; }
        public string password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
