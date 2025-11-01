using Durga.Api.Domain.Entities;

namespace Durga.Api.Application.Ports;

public interface IDepartmentRepository
{
    Task<Department?> GetByIdAsync(Guid id);
    Task<Department?> GetByNameAsync(string name);
    Task<IEnumerable<Department>> GetAllAsync();
    Task<IEnumerable<Department>> GetWithManagersAsync();
    Task<Department> CreateAsync(Department department);
    Task<Department> UpdateAsync(Department department);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);
    Task<IEnumerable<Team>> GetDepartmentTeamsAsync(Guid departmentId);
    Task<int> GetTeamCountAsync(Guid departmentId);
}