using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IPP.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("TbEmployees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(e => new { e.CompanyId, e.Email })
               .IsUnique();

        builder.Property(e => e.HireDate)
            .IsRequired();

        builder.HasOne(e => e.Company)
               .WithMany(c => c.Employees)
               .HasForeignKey(e => e.CompanyId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}