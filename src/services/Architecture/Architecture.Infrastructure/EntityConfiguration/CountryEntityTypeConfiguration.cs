using Architecture.Domain.AggregatesModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Architecture.Infrastructure.EntityConfiguration
{
    public class CountryEntityTypeConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> tableConfiguration)
        {
            tableConfiguration.ToTable("Country");
            tableConfiguration.HasKey(c => c.Id);
            tableConfiguration.Property(c => c.Id).ValueGeneratedOnAdd();
                //.UseHiLo("Country_seq");

            tableConfiguration.Ignore(c => c.DomainEvents);
            // Other columns: -----------------------------------------------------

            tableConfiguration.Property(c => c.Name).IsRequired().HasMaxLength(50);
            tableConfiguration.Property(c => c.Code).IsRequired().HasMaxLength(255);
        }
    }
}
