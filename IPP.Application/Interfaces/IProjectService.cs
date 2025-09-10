using Application.Projects.Commands;
using Application.Responses.Common;
using Application.Responses.Projects;
using Application.Projects.Queries;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<PagedResponse<ProjectResponse>> GetProjectsAsync(GetProjectsQuery query);
    Task<ProjectResponse?> GetProjectByIdAsync(GetProjectByIdQuery query);
    Task<ProjectResponse> CreateProjectAsync(CreateProjectCommand command);
    Task<ProjectResponse?> UpdateProjectAsync(UpdateProjectCommand command);
    Task<bool> DeleteProjectAsync(DeleteProjectCommand command);
}
