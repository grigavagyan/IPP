using Application.Responses.Common;
using Application.Responses.Projects;
using IPP.Application.Interfaces;
using IPP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Projects.Queries.GetProjects;

public class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, PagedResponse<ProjectResponse>>
{
    private readonly DataContext _dataContext;
    public GetProjectsQueryHandler(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<PagedResponse<ProjectResponse>> Handle(GetProjectsQuery query, CancellationToken cancellation)
    {
        var projects = _dataContext.Projects.AsQueryable();

        // ToDo Change
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
}
