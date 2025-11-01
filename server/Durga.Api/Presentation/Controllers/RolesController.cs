using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Durga.Api.Application.Ports;
using Durga.Api.Application.DTOs;
using Durga.Api.Application.DTOs.Roles;

namespace Durga.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<RolesController> _logger;

    public RolesController(IRoleRepository roleRepository, ILogger<RolesController> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all roles with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="searchTerm">Search term to filter roles</param>
    /// <param name="includeInactive">Include inactive roles</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of roles</returns>
    [HttpGet]
    public async Task<ActionResult<RoleListDto>> GetRoles(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool includeInactive = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate pagination parameters
            pageNumber = Math.Max(1, pageNumber);
            pageSize = Math.Min(100, Math.Max(1, pageSize));

            var pagedResult = await _roleRepository.GetPagedAsync(
                pageNumber, pageSize, searchTerm, includeInactive, cancellationToken);

            var roleListDto = pagedResult.ToListDto(pageNumber, pageSize);

            return Ok(roleListDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles with pagination");
            return StatusCode(500, new { message = "An error occurred while retrieving roles" });
        }
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Role details</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoleDto>> GetRole(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await _roleRepository.GetByIdAsync(id, cancellationToken);
            
            if (role == null)
            {
                return NotFound(new { message = "Role not found" });
            }

            var roleDto = role.ToDto();
            return Ok(roleDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving role {RoleId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the role" });
        }
    }

    /// <summary>
    /// Get role by name
    /// </summary>
    /// <param name="name">Role name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Role details</returns>
    [HttpGet("by-name/{name}")]
    public async Task<ActionResult<RoleDto>> GetRoleByName(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await _roleRepository.GetByNameAsync(name, cancellationToken);
            
            if (role == null)
            {
                return NotFound(new { message = "Role not found" });
            }

            var roleDto = role.ToDto();
            return Ok(roleDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving role by name {RoleName}", name);
            return StatusCode(500, new { message = "An error occurred while retrieving the role" });
        }
    }

    /// <summary>
    /// Get active roles only
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active roles</returns>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<RoleSummaryDto>>> GetActiveRoles(CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _roleRepository.GetActiveRolesAsync(cancellationToken);
            var roleSummaries = roles.Select(r => r.ToSummaryDto()).ToList();

            return Ok(roleSummaries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active roles");
            return StatusCode(500, new { message = "An error occurred while retrieving active roles" });
        }
    }

    /// <summary>
    /// Get system roles
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of system roles</returns>
    [HttpGet("system")]
    public async Task<ActionResult<IEnumerable<RoleSummaryDto>>> GetSystemRoles(CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _roleRepository.GetSystemRolesAsync(cancellationToken);
            var roleSummaries = roles.Select(r => r.ToSummaryDto()).ToList();

            return Ok(roleSummaries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving system roles");
            return StatusCode(500, new { message = "An error occurred while retrieving system roles" });
        }
    }

    /// <summary>
    /// Get user-defined roles
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of user-defined roles</returns>
    [HttpGet("user-defined")]
    public async Task<ActionResult<IEnumerable<RoleSummaryDto>>> GetUserDefinedRoles(CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _roleRepository.GetUserDefinedRolesAsync(cancellationToken);
            var roleSummaries = roles.Select(r => r.ToSummaryDto()).ToList();

            return Ok(roleSummaries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user-defined roles");
            return StatusCode(500, new { message = "An error occurred while retrieving user-defined roles" });
        }
    }

    /// <summary>
    /// Get users in a role
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of users in the role</returns>
    [HttpGet("{id:guid}/users")]
    public async Task<ActionResult<IEnumerable<UserSummaryDto>>> GetRoleUsers(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // First check if role exists
            var roleExists = await _roleRepository.ExistsAsync(id, cancellationToken);
            if (!roleExists)
            {
                return NotFound(new { message = "Role not found" });
            }

            var users = await _roleRepository.GetRoleUsersAsync(id, cancellationToken);
            var userSummaries = users.Select(u => u.ToUserSummaryDto()).ToList();

            return Ok(userSummaries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users for role {RoleId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving role users" });
        }
    }

    /// <summary>
    /// Get users in a role by role name
    /// </summary>
    /// <param name="name">Role name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of users in the role</returns>
    [HttpGet("by-name/{name}/users")]
    public async Task<ActionResult<IEnumerable<UserSummaryDto>>> GetRoleUsersByName(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            // First check if role exists
            var roleExists = await _roleRepository.NameExistsAsync(name, cancellationToken);
            if (!roleExists)
            {
                return NotFound(new { message = "Role not found" });
            }

            var users = await _roleRepository.GetRoleUsersAsync(name, cancellationToken);
            var userSummaries = users.Select(u => u.ToUserSummaryDto()).ToList();

            return Ok(userSummaries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users for role {RoleName}", name);
            return StatusCode(500, new { message = "An error occurred while retrieving role users" });
        }
    }

    /// <summary>
    /// Get user count in a role
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of users in the role</returns>
    [HttpGet("{id:guid}/user-count")]
    public async Task<ActionResult<int>> GetUserCountInRole(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // First check if role exists
            var roleExists = await _roleRepository.ExistsAsync(id, cancellationToken);
            if (!roleExists)
            {
                return NotFound(new { message = "Role not found" });
            }

            var userCount = await _roleRepository.GetUserCountInRoleAsync(id, cancellationToken);
            return Ok(userCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user count for role {RoleId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving user count" });
        }
    }
}