using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Unistream.Transactions.Model.EF.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Unistream.Transactions.Model.EF.EntityTypeConfiguration
{
    internal class TransactionHistoryConfiguration : IEntityTypeConfiguration<TransactionHistory>
    {
        public void Configure(EntityTypeBuilder<TransactionHistory> builder)
        {
            builder.ToTable("Transactions", "info");

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

            builder.Property(x => x.TransactionType).HasColumnName(@"TransactionType").HasColumnType("smallint").IsRequired();
            builder.Property(x => x.Amount).HasColumnName(@"Amount").HasColumnType("decimal").IsRequired();
            builder.Property(x => x.ClientId).HasColumnName(@"ClientId").HasColumnType("uuid").IsRequired();
            builder.Property(x => x.TransactionDateTime).HasColumnName(@"TransactionDateTime").HasColumnType("timestamp(6) with time zone").IsRequired();
            builder.Property(x => x.IsRollback).HasColumnName(@"IsRollback").HasColumnType("boolean");
            builder.Property(x => x.RollbackDateTime).HasColumnName(@"RollbackDateTime").HasColumnType("timestamp(6) with time zone");
        }
    }
}
