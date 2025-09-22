using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RiderGo.Domain.Entities;

namespace RiderGo.Infrastructure.Mappings
{
    public class RentalMapping : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("rentals")
                .HasKey(x => x.Id)
                .HasName("pk_rental");

            builder.Property(x => x.RiderId)
                .HasColumnName("rider_id")
                .IsRequired();

            builder.Property(x => x.MotorcycleId)
                .HasColumnName("motorcycle_id")
                .IsRequired();

            builder.Property(x => x.StartDate)
                .HasColumnName("start_date")
                .IsRequired();

            builder.Property(x => x.EndDate)
                .HasColumnName("end_date")
                .IsRequired();

            builder.Property(x => x.ExpectedEndDate)
                .HasColumnName("expected_end_date")
                .IsRequired();

            builder.Property(x => x.Plan)
                .HasColumnName("plan")
                .IsRequired();

            builder.Property(x => x.DailyRate)
                .HasColumnName("daily_rate")
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.TotalAmount)
                .HasColumnName("total_amount")
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.ReturnDate)
                .HasColumnName("return_date")
                .IsRequired(false);


            // Relationships

            builder.HasOne(x => x.Rider)
                .WithMany(x => x.Rentals)
                .HasForeignKey(x => x.RiderId)
                .HasConstraintName("fk_rental_rider");

            builder.HasOne(x => x.Motorcycle)
                .WithMany(x => x.Rentals)
                .HasForeignKey(x => x.MotorcycleId)
                .HasConstraintName("fk_rental_motorcycle");
        }
    }
}
