using Architecture.Domain.SeedWork;
using Architecture.Infrastruture;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Architecture.Infrastructure
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, Entity entity)
        {
            if (entity.DomainEvents != null && entity.DomainEvents.Any())
            {
                var domainEvents = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();
                var tasks = domainEvents.Select(async (domainEvent) =>
                {
                    await mediator.Publish(domainEvent);
                });
                await Task.WhenAll(tasks);
            }
        }
    }
}
