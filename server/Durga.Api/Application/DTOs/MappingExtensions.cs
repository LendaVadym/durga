using Durga.Api.Domain.Entities;
using Durga.Api.Application.DTOs.Users;
using Durga.Api.Application.DTOs.Roles;

namespace Durga.Api.Application.DTOs;

public static class MappingExtensions
{
    // User mappings
    public static Users.UserDto ToDto(this User user)
    {
        return new Users.UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            TwoFactorEnabled = user.TwoFactorEnabled,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            AccessFailedCount = user.AccessFailedCount,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
            ProfileImageUrl = user.ProfileImageUrl,
            IsActive = user.IsActive,
            IsEmailVerified = user.IsEmailVerified,
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt,
            CreatedBy = user.CreatedBy,
            UpdatedAt = user.UpdatedAt,
            UpdatedBy = user.UpdatedBy,
            Roles = user.UserRoles
                .Where(ur => ur.IsValidAssignment)
                .Select(ur => ur.Role.ToRoleDto())
                .ToList()
        };
    }

    public static Users.UserSummaryDto ToSummaryDto(this User user)
    {
        return new Users.UserSummaryDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            IsActive = user.IsActive,
            EmailConfirmed = user.EmailConfirmed,
            CreatedAt = user.CreatedAt,
            RoleNames = user.UserRoles
                .Where(ur => ur.IsValidAssignment)
                .Select(ur => ur.Role.Name)
                .ToList()
        };
    }

    public static UserListDto ToListDto(this (IEnumerable<User> Users, int TotalCount) pagedResult, int pageNumber, int pageSize)
    {
        return new UserListDto
        {
            Users = pagedResult.Users.Select(u => u.ToSummaryDto()).ToList(),
            TotalCount = pagedResult.TotalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    // Role mappings
    public static Roles.RoleDto ToDto(this Role role)
    {
        return new Roles.RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsSystemRole = role.IsSystemRole,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt,
            CreatedBy = role.CreatedBy,
            UpdatedAt = role.UpdatedAt,
            UpdatedBy = role.UpdatedBy,
            UserCount = role.UserRoles.Count(ur => ur.IsValidAssignment && !ur.User.IsDeleted),
            Users = role.UserRoles
                .Where(ur => ur.IsValidAssignment && !ur.User.IsDeleted)
                .Select(ur => ur.User.ToUserSummaryDto())
                .ToList()
        };
    }

    public static RoleSummaryDto ToSummaryDto(this Role role)
    {
        return new RoleSummaryDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsSystemRole = role.IsSystemRole,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt,
            UserCount = role.UserRoles.Count(ur => ur.IsValidAssignment && !ur.User.IsDeleted)
        };
    }

    public static RoleListDto ToListDto(this (IEnumerable<Role> Roles, int TotalCount) pagedResult, int pageNumber, int pageSize)
    {
        return new RoleListDto
        {
            Roles = pagedResult.Roles.Select(r => r.ToSummaryDto()).ToList(),
            TotalCount = pagedResult.TotalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    // Helper methods for cross-referencing
    public static Users.RoleDto ToRoleDto(this Role role)
    {
        return new Users.RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            IsSystemRole = role.IsSystemRole,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt,
            CreatedBy = role.CreatedBy
        };
    }

    public static Roles.UserSummaryDto ToUserSummaryDto(this User user)
    {
        return new Roles.UserSummaryDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}