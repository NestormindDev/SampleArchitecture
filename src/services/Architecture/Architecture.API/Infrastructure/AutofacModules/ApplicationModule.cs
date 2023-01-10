using Autofac;
using Architecture.API.Application.Queries;
using Architecture.Domain.AggregatesModel;
using Architecture.Infrastructure.Helpers;
using Architecture.Infrastruture.Repositories;
using Microsoft.Graph;
using Microsoft.Identity.Client;

namespace Architecture.API.Infrastructure.AutofacModules
{

    public class ApplicationModule : Autofac.Module
    {
        private readonly string dbConnStr;


        public ApplicationModule(string dbConnStr)
        {
            this.dbConnStr = dbConnStr;
        }

        protected override void Load(ContainerBuilder builder)
        {

            //Queries

            builder.Register(c => new CountryQueries(dbConnStr)).As<ICountryQueries>().InstancePerLifetimeScope();
            builder.Register(c => new LocationQueries(dbConnStr)).As<ILocationQueries>().InstancePerLifetimeScope();

            //Repositories
            builder.RegisterType<CountryRepository>().As<ICountryRepository>().InstancePerLifetimeScope();

        }
    }
}
