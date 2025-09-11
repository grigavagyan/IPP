using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IPP.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("TbProjects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => new { p.CompanyId, p.Name })
               .IsUnique();

        builder.Property(p => p.StartDate).IsRequired();
        builder.Property(p => p.EndDate);

        builder.HasOne(p => p.Company)
               .WithMany(c => c.Projects)
               .HasForeignKey(p => p.CompanyId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}