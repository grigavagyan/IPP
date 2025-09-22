using Application.Responses.Common;
using Application.Responses.Projects;
using Domain.Entities;
using Domain.Enums;
using IPP.Application.Projects.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IPP.Application.Projects.GetProjects;

public class GetProjectsQueryHandler : IQueryHandler<GetProjectsQuery, PagedResponse<ProjectResponse>>
{
    private readonly IRepository<Project> _repository;
    public GetProjectsQueryHandler(IRepository<Project> repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<ProjectResponse>> Handle(GetProjectsQuery query, CancellationToken cancellation)
    {
        var projectQuery = _repository.Query().Where(x => x.CompanyId == query.CompanyId).AsNoTracking();

        if (query.SortOrder == SortOrder.Desc)
            projectQuery.OrderByDescending(x => x.Name);

        if (!string.IsNullOrEmpty(query.SearchText))
        {
            var parts = query.SearchText
                .Split([" ", ", "], StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower());

            foreach (var part in parts)
                projectQuery = projectQuery.Where(c => c.Name.ToLower().Contains(part));
        }

        if (query.StartDateFrom.HasValue)
            projectQuery = projectQuery.Where(c => c.StartDate >= query.StartDateFrom.Value);
        
        if (query.StartDateTo.HasValue)
            projectQuery = projectQuery.Where(c => c.StartDate <= query.StartDateTo.Value);

        var total = await projectQuery.CountAsync(cancellation);
        var items = await projectQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new ProjectResponse
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