using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IPP.Infrastructure.Configurations;

public class EmployeeProjectConfiguration : IEntityTypeConfiguration<EmployeeProject>
{
    public void Configure(EntityTypeBuilder<EmployeeProject> builder)
    {
        builder.ToTable("TbEmployeeProjects");

        builder.HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

        builder.Property(ep => ep.AssignedDate)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(ep => ep.Employee)
               .WithMany(e => e.EmployeeProjects)
               .HasForeignKey(ep => ep.EmployeeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ep => ep.Project)
               .WithMany(p => p.EmployeeProjects)
               .HasForeignKey(ep => ep.ProjectId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}