using Durga.Api.Application.DTOs.Users;
using Durga.Api.Application.DTOs.Departments;

namespace Durga.Api.Application.DTOs.Teams;

public record TeamDto(
    Guid Id,
    string Name,
    string? Description,
    Guid DepartmentId,
    string DepartmentName,
    Guid? LeaderId,
    string? LeaderName,
    Guid? ManagerId,
    string? ManagerName,
    bool IsActive,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    int ActiveMemberCount
);

public record CreateTeamDto(
    string Name,
    string? Description,
    Guid DepartmentId,
    Guid? LeaderId,
    Guid? ManagerId
);

public record UpdateTeamDto(
    string Name,
    string? Description,
    Guid? LeaderId,
    Guid? ManagerId,
    bool IsActive
);

public record TeamListItemDto(
    Guid Id,
    string Name,
    string DepartmentName,
    string? LeaderName,
    string? ManagerName,
    int ActiveMemberCount,
    bool IsActive,
    DateTime CreatedAt
);

public record TeamSummaryDto(
    Guid Id,
    string Name,
    string DepartmentName,
    int MemberCount
);

public record TeamMemberDto(
    Guid UserId,
    string UserName,
    string Email,
    string FullName,
    DateTime JoinedAt,
    bool IsActive,
    bool IsLeader,
    bool IsManager
);