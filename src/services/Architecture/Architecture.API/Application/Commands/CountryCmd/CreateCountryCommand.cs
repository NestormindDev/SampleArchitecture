using MediatR;
using Newtonsoft.Json;
using Architecture.API.Application.Models;
using Architecture.API.Application.Queries;

namespace Architecture.API.Application.Commands
{
    public class CreateCountryCommand : IRequest<bool>
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; } 

        public CreateCountryCommand(int id, string name, string code)
        {
            this.Id = id;
            this.Name = name;
            this.Code = code; 
        }
    }
}
