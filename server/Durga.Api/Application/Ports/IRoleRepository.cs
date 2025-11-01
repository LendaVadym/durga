using Durga.Api.Domain.Entities;

namespace Durga.Api.Application.Ports;

public interface IRoleRepository
{
    // Read operations
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetActiveRolesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetUserDefinedRolesAsync(CancellationToken cancellationToken = default);
    
    // Role user operations
    Task<IEnumerable<User>> GetRoleUsersAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetRoleUsersAsync(string roleName, CancellationToken cancellationToken = default);
    Task<int> GetUserCountInRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<int> GetUserCountInRoleAsync(string roleName, CancellationToken cancellationToken = default);
    
    // Read operations with pagination
    Task<(IEnumerable<Role> Roles, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? searchTerm = null,
        bool includeInactive = false,
        CancellationToken cancellationToken = default);
    
    // Existence checks
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);
}