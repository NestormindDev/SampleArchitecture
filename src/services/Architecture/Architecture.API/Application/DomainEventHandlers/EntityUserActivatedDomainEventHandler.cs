using Architecture.API.Application.Queries;
using Architecture.Domain.AggregatesModel;
using Architecture.Infrastructure.Helpers;
using MediatR;
using Microsoft.Extensions.Configuration;
using Survey.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Architecture.API.Application.DomainEventHandlers
{
    public class EntityUserActivatedDomainEventHandler : INotificationHandler<EntityUserActivatedDomainEvent>
    {
        private readonly IConfiguration _configuration;

        public EntityUserActivatedDomainEventHandler(IConfiguration configuration)
        {
          
            _configuration = configuration;
        }

        public async Task Handle(EntityUserActivatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // Raise domain event to send the activation email to the user.
        }
    }
}