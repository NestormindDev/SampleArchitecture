using AutoMapper;
using MediatR;
using Architecture.Domain.AggregatesModel;
using Architecture.Domain.SeedWork;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Serilog;
using Architecture.Domain.Extensions;

namespace Architecture.API.Application.Commands
{
    public class ReplaceCountryCommandHandler : IRequestHandler<ReplaceCountryCommand, bool>
    {
        private readonly ICountryRepository _repository;
        private readonly IMapper _mapper;

        public ReplaceCountryCommandHandler(ICountryRepository CountryRepository, IMapper mapper)
        {
            _repository = CountryRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(ReplaceCountryCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var storedEntity = await _repository.FindByIdAsync(cmd.Entity.Id);                

                if (storedEntity == null)
                    return false;

                var cmdEntity = _mapper.Map(cmd.Entity, storedEntity);

                _repository.Update(cmdEntity);

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
