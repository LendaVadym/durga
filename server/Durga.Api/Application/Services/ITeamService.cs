using Durga.Api.Application.DTOs.Teams;
using Durga.Api.Application.DTOs.Users;

namespace Durga.Api.Application.Services;

public interface ITeamService
{
    Task<TeamDto> GetTeamByIdAsync(Guid id);
    Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
    Task<IEnumerable<TeamDto>> GetTeamsByDepartmentAsync(Guid departmentId);
    Task<TeamDto> CreateTeamAsync(CreateTeamDto createTeamDto);
    Task<TeamDto> UpdateTeamAsync(Guid id, UpdateTeamDto updateTeamDto);
    Task<bool> DeleteTeamAsync(Guid id);
    Task<bool> AssignLeaderAsync(Guid teamId, Guid leaderId);
    Task<bool> AssignManagerAsync(Guid teamId, Guid managerId);
    Task<bool> AddTeamMemberAsync(Guid teamId, Guid userId, Guid addedBy);
    Task<bool> RemoveTeamMemberAsync(Guid teamId, Guid userId, Guid removedBy);
    Task<IEnumerable<UserDto>> GetTeamMembersAsync(Guid teamId);
    Task<int> GetTeamMemberCountAsync(Guid teamId);
}