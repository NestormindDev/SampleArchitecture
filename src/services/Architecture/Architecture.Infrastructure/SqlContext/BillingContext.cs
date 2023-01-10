using Architecture.Domain.AggregatesModel; 
using Architecture.Domain.SeedWork;
using Architecture.Infrastructure;
using Architecture.Infrastructure.EntityConfiguration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Architecture.Infrastruture
{
    public class ArchitectureContext : DbContext, IUnitOfWork
    {
        
        public DbSet<Country> Countries { get; set; }
        
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction => _currentTransaction;

        public ArchitectureContext()
        { }

        public ArchitectureContext(DbContextOptions<ArchitectureContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public ArchitectureContext(DbContextOptions<ArchitectureContext> options, IMediator mediator, IConfiguration configuration) : base(options)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CountryEntityTypeConfiguration()); 
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync();

            return true;
        }

        public async Task BeginTransactionAsync()
        {
            _currentTransaction = _currentTransaction ?? await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }

    public class ArchitectureContextDesignFactory : IDesignTimeDbContextFactory<ArchitectureContext>
    {
        public ArchitectureContext CreateDbContext(string[] args)
        {
            var AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../" + AssemblyName);

            IConfiguration configuration = new ConfigurationBuilder()
               .SetBasePath(basePath)
               .AddJsonFile("appsettings.json")
               .Build();

            var connectionString = configuration["ConnectionString"];

            var optionsBuilder = new DbContextOptionsBuilder<ArchitectureContext>().UseSqlServer(connectionString);

            return new ArchitectureContext(optionsBuilder.Options, new NoMediator(), configuration);
        }

        class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TResponse>(default(TResponse));
            }

            public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }

            public Task<object> Send(object request, CancellationToken cancellationToken = default)
            {
                return null;
            }
        }
    }
}
