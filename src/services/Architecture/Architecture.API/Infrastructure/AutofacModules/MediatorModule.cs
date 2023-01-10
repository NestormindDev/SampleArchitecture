using Autofac;
using Architecture.API.Application.Behaviors;
using Architecture.API.Application.Commands;
using Architecture.API.Application.DomainEventHandlers;
using Architecture.API.Application.Validation;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Architecture.API.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            
            builder.RegisterAssemblyTypes(typeof(CreateCountryCommand).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
            

            // Register the DomainEventHandler classes: this is to propagate change, used for Out-of-Sync remediation
            builder.RegisterAssemblyTypes(typeof(EntityUserActivatedDomainEventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(INotificationHandler<>));
            

            // Register the Command's Validators 
                   builder.RegisterAssemblyTypes(typeof(CreateCountryCommandValidator).GetTypeInfo().Assembly).Where(t => t.IsClosedTypeOf(typeof(IValidator<>))).AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        }
    }
}
