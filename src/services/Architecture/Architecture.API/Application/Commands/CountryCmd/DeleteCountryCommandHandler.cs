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
    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, bool>
    {
        private readonly ICountryRepository _repository;

        public DeleteCountryCommandHandler(ICountryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.FindByIdAsync(request.Id);

                _repository.Delete(entity);

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
