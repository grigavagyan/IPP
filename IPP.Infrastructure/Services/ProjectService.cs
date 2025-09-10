using Application.Interfaces;
using Application.Projects.Commands;
using Application.Projects.Queries;
using Application.Responses.Common;
using Application.Responses.Projects;
using Domain.Entities;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _db;

    public ProjectService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResponse<ProjectResponse>> GetProjectsAsync(GetProjectsQuery query)
    {
        var projects = _db.Projects.AsQueryable();

        // Filtering
        if (query.CompanyId.HasValue)
            projects = projects.Where(p => p.CompanyId == query.CompanyId.Value);

        if (!string.IsNullOrEmpty(query.NameContains))
            projects = projects.Where(p => p.Name.Contains(query.NameContains));

        if (query.StartDateFrom.HasValue)
            projects = projects.Where(p => p.StartDate >= query.StartDateFrom.Value);

        if (query.StartDateTo.HasValue)
            projects = projects.Where(p => p.StartDate <= query.StartDateTo.Value);

        if (query.ActiveOnly)
            projects = projects.Where(p => !p.EndDate.HasValue || p.EndDate >= DateTime.UtcNow);

        var total = await projects.CountAsync();

        // Pagination
        projects = projects.Skip((query.Page - 1) * query.PageSize)
                           .Take(query.PageSize);

        var items = await projects.Select(p => new ProjectResponse
        {
            Id = p.Id,
            CompanyId = p.CompanyId,
            Name = p.Name,
            StartDate = p.StartDate,
            EndDate = p.EndDate
        }).ToListAsync();

        return new PagedResponse<ProjectResponse>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    public async Task<ProjectResponse?> GetProjectByIdAsync(GetProjectByIdQuery query)
    {
        var project = await _db.Projects.FindAsync(query.Id);
        if (project == null) return null;

        return new ProjectResponse
        {
            Id = project.Id,
            CompanyId = project.CompanyId,
            Name = project.Name,
            StartDate = project.StartDate,
            EndDate = project.EndDate
        };
    }

    public async Task<ProjectResponse> CreateProjectAsync(CreateProjectCommand command)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            CompanyId = command.CompanyId,
            Name = command.Name,
            StartDate = command.StartDate,
            EndDate = command.EndDate
        };

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        return new ProjectResponse
        {
            Id = project.Id,
            CompanyId = project.CompanyId,
            Name = project.Name,
            StartDate = project.StartDate,
            EndDate = project.EndDate
        };
    }

    public async Task<ProjectResponse?> UpdateProjectAsync(UpdateProjectCommand command)
    {
        var project = await _db.Projects.FindAsync(command.Id);
        if (project == null) return null;

        project.CompanyId = command.CompanyId;
        project.Name = command.Name;
        project.StartDate = command.StartDate;
        project.EndDate = command.EndDate;

        await _db.SaveChangesAsync();

        return new ProjectResponse
        {
            Id = project.Id,
            CompanyId = project.CompanyId,
            Name = project.Name,
            StartDate = project.StartDate,
            EndDate = project.EndDate
        };
    }

    public async Task<bool> DeleteProjectAsync(DeleteProjectCommand command)
    {
        var project = await _db.Projects.FindAsync(command.Id);
        if (project == null) return false;

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
        return true;
    }
}
