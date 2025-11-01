using Durga.Api.Domain.Entities;

namespace Durga.Api.Application.Ports;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(Guid id);
    Task<Team?> GetByNameAsync(string name);
    Task<IEnumerable<Team>> GetAllAsync();
    Task<IEnumerable<Team>> GetByDepartmentAsync(Guid departmentId);
    Task<IEnumerable<Team>> GetActiveTeamsAsync();
    Task<Team> CreateAsync(Team team);
    Task<Team> UpdateAsync(Team team);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);
    Task<IEnumerable<User>> GetTeamMembersAsync(Guid teamId);
    Task<IEnumerable<User>> GetActiveTeamMembersAsync(Guid teamId);
    Task<int> GetMemberCountAsync(Guid teamId);
    Task<int> GetActiveMemberCountAsync(Guid teamId);
    Task<bool> AddMemberAsync(Guid teamId, Guid userId, Guid addedBy);
    Task<bool> RemoveMemberAsync(Guid teamId, Guid userId, Guid removedBy);
    Task<bool> IsUserInTeamAsync(Guid teamId, Guid userId);
}