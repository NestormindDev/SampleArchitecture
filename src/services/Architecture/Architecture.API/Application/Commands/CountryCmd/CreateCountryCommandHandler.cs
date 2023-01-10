using MediatR;
using Architecture.Domain.AggregatesModel;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using Architecture.Domain.Extensions;
using System;

namespace Architecture.API.Application.Commands
{
    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, bool>
    {
        private readonly ICountryRepository _repository;


        public CreateCountryCommandHandler(ICountryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = new Country(request.Name, request.Code);

                _repository.Add(entity);

                return await _repository.UnitOfWork.SaveEntitiesAsync();
            }
            catch (Exception ex)
            {
                Log.Logger.Here().Error($"Exception: {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}
