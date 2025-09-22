using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RiderGo.Domain.Entities;

namespace RiderGo.Infrastructure.Mappings
{
    public class MotorcycleMapping : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.ToTable("motorcycles")
                .HasKey(x => x.Id)
                .HasName("pk_motorcycle");

            builder.Property(x => x.Model)
                .HasColumnName("model")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Plate)
                .HasColumnName("plate")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Year)
                .HasColumnName("year")
                .IsRequired();

            builder.Property(x => x.IsFrom2024)
                .HasColumnName("is_from_2024")
                .IsRequired(false);


            builder.HasIndex(x => x.Plate)
                .IsUnique()
                .HasDatabaseName("idx_motorcycle_plate");
        }
    }
}
