using Microsoft.EntityFrameworkCore;
using Durga.Api.Application.Ports;
using Durga.Api.Domain.Entities;
using Durga.Api.Infrastructure.Adapters.Persistence;

namespace Durga.Api.Infrastructure.Adapters.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DurgaDbContext _context;

    public RoleRepository(DurgaDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
                .ThenInclude(ur => ur.User)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
                .ThenInclude(ur => ur.User)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
                .ThenInclude(ur => ur.User)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetActiveRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
                .ThenInclude(ur => ur.User)
            .Where(r => r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetSystemRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
                .ThenInclude(ur => ur.User)
            .Where(r => r.IsSystemRole && r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetUserDefinedRolesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
                .ThenInclude(ur => ur.User)
            .Where(r => !r.IsSystemRole && r.IsActive)
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetRoleUsersAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted && 
                   u.UserRoles.Any(ur => ur.RoleId == roleId && ur.IsValidAssignment))
            .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetRoleUsersAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Where(u => !u.IsDeleted && 
                   u.UserRoles.Any(ur => ur.Role.Name == roleName && ur.IsValidAssignment))
            .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUserCountInRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .Where(ur => ur.RoleId == roleId && ur.IsValidAssignment)
            .Select(ur => ur.User)
            .Where(u => !u.IsDeleted)
            .CountAsync(cancellationToken);
    }

    public async Task<int> GetUserCountInRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .Where(ur => ur.Role.Name == roleName && ur.IsValidAssignment)
            .Select(ur => ur.User)
            .Where(u => !u.IsDeleted)
            .CountAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Role> Roles, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? searchTerm = null, 
        bool includeInactive = false, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<Role> query = _context.Roles
            .Include(r => r.UserRoles)
                .ThenInclude(ur => ur.User);

        if (!includeInactive)
        {
            query = query.Where(r => r.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearchTerm = searchTerm.ToLower();
            query = query.Where(r => 
                r.Name.ToLower().Contains(lowerSearchTerm) ||
                (r.Description != null && r.Description.ToLower().Contains(lowerSearchTerm)));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        
        var roles = await query
            .OrderBy(r => r.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (roles, totalCount);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AnyAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .AnyAsync(r => r.Name == name, cancellationToken);
    }
}