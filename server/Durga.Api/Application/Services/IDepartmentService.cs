using Durga.Api.Application.DTOs.Departments;
using Durga.Api.Application.DTOs.Teams;

namespace Durga.Api.Application.Services;

public interface IDepartmentService
{
    Task<DepartmentDto> GetDepartmentByIdAsync(Guid id);
    Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
    Task<DepartmentDto> CreateDepartmentAsync(CreateDepartmentDto createDepartmentDto);
    Task<DepartmentDto> UpdateDepartmentAsync(Guid id, UpdateDepartmentDto updateDepartmentDto);
    Task<bool> DeleteDepartmentAsync(Guid id);
    Task<bool> AssignManagerAsync(Guid departmentId, Guid managerId);
    Task<IEnumerable<TeamDto>> GetDepartmentTeamsAsync(Guid departmentId);
}