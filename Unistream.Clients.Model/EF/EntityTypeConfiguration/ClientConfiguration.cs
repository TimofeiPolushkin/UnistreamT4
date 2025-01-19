using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Unistream.Clients.Model.EF.Entities;

namespace Unistream.Clients.Model.EF.EntityTypeConfiguration
{
    internal class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients", "info");

            builder.Property(x => x.Id).HasColumnName(@"Id").IsRequired().HasColumnType("bigint").UseIdentityColumn();
            builder.Property(x => x.UniId).HasColumnName(@"UniId").HasColumnType("uuid").IsRequired();
            builder.Property(x => x.Created).HasColumnName(@"Created").HasColumnType("timestamp(6) with time zone");
            builder.Property(x => x.Updated).HasColumnName(@"Updated").HasColumnType("timestamp(6) with time zone");
            builder.Property(x => x.Deleted).HasColumnName(@"Deleted").HasColumnType("timestamp(6) with time zone");

            builder.Property(x => x.Version)
                    .HasColumnName("xmin")
                    .HasColumnType("xid")
                    .HasConversion(new ValueConverter<byte[], long>(
                        value => BitConverter.ToInt64(value, 0),
                        value => BitConverter.GetBytes(value)))
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("varchar(255)").IsRequired();
            builder.Property(x => x.MiddleName).HasColumnName(@"MiddleName").HasColumnType("varchar(255)").IsRequired();
            builder.Property(x => x.LastName).HasColumnName(@"LastName").HasColumnType("varchar(255)").IsRequired();
            builder.Property(x => x.Balance).HasColumnName(@"Balance").HasColumnType("decimal").IsRequired();
        }
    }
}
