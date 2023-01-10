using MediatR;
using Newtonsoft.Json;
using Architecture.API.Application.Models;
using Architecture.Domain.AggregatesModel;

namespace Architecture.API.Application.Commands
{
    public class ReplaceCountryCommand : IRequest<bool>
    {
        public CountryDto Entity { get; private set; }

        public ReplaceCountryCommand(CountryDto entity)
        {
            Entity = entity;
        }
    }
}
