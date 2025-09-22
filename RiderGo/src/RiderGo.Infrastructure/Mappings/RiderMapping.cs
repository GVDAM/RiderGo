using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RiderGo.Domain.Entities;

namespace RiderGo.Infrastructure.Mappings
{
    public class RiderMapping : IEntityTypeConfiguration<Rider>
    {
        public void Configure(EntityTypeBuilder<Rider> builder)
        {
            builder.ToTable("riders")
                .HasKey(x => x.Id)
                .HasName("pk_rider");

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.CNPJ)
                .HasColumnName("cnpj")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Birth)
                .HasColumnName("birth")
                .IsRequired();

            builder.Property(x => x.CnhNumber)
                .HasColumnName("cnh_number")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(x => x.CnhType)
                .HasColumnName("cnh_type")
                .HasColumnType("varchar")
                .HasMaxLength(2)
                .IsRequired();

            builder.Property(x => x.CnhImageUrl)
                .HasColumnName("cnh_image_url")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => x.CNPJ)
                .IsUnique()
                .HasDatabaseName("idx_rider_cnpj");

            builder.HasIndex(x => x.CnhNumber)
                .IsUnique()
                .HasDatabaseName("idx_rider_cnh_number");
        }
    }
}
