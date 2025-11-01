using Durga.Api.Application.DTOs.Users;

namespace Durga.Api.Application.Services;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task<PagedResultDto<UserListItemDto>> GetUsersAsync(int pageNumber, int pageSize, string? searchTerm = null, bool includeInactive = false);
    Task<IEnumerable<UserListItemDto>> GetActiveUsersAsync();
    Task<IEnumerable<UserListItemDto>> GetUsersByRoleAsync(string roleName);
    Task<IEnumerable<RoleDto>> GetUserRolesAsync(Guid userId);
    Task<bool> IsUserInRoleAsync(Guid userId, string roleName);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto updateUserDto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<bool> AssignUserToTeamAsync(Guid userId, Guid teamId, Guid assignedBy);
    Task<bool> RemoveUserFromTeamAsync(Guid userId, Guid teamId, Guid removedBy);
}