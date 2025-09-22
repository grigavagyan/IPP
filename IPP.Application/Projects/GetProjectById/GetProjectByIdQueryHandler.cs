using Application.Responses.Projects;
using Domain.Entities;
using IPP.Application.Exceptions;
using IPP.Application.Projects.Interfaces;

namespace IPP.Application.Projects.GetProjectById;

public class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
{
    private readonly IRepository<Project> _repository;
    public GetProjectByIdQueryHandler(IRepository<Project> repository)
    {
        _repository = repository;
    }

    public async Task<ProjectResponse> Handle(GetProjectByIdQuery query, CancellationToken cancellation)
    {
        var project = await _repository.GetByIdAsync(query.Id);

        if (project == null)
            throw new NotFoundException($"Project with id {query.Id} not found.");

        return new ProjectResponse
        {
            Id = project.Id,
            CompanyId = project.CompanyId,
            Name = project.Name,
            StartDate = project.StartDate,
            EndDate = project.EndDate
        };
    }
}