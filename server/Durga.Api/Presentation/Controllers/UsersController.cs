using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Durga.Api.Application.Ports;
using Durga.Api.Application.DTOs;
using Durga.Api.Application.DTOs.Users;

namespace Durga.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <summary>
    /// Get all users with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="searchTerm">Search term to filter users</param>
    /// <param name="includeInactive">Include inactive users</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of users</returns>
    [HttpGet]
    public async Task<ActionResult<UserListDto>> GetUsers(
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

            var pagedResult = await _userRepository.GetPagedAsync(
                pageNumber, pageSize, searchTerm, includeInactive, cancellationToken);

            var userListDto = pagedResult.ToListDto(pageNumber, pageSize);

            return Ok(userListDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users with pagination");
            return StatusCode(500, new { message = "An error occurred while retrieving users" });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User details</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userDto = user.ToDto();
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the user" });
        }
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    /// <param name="email">User email</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User details</returns>
    [HttpGet("by-email/{email}")]
    public async Task<ActionResult<UserDto>> GetUserByEmail(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var userDto = user.ToDto();
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user by email {Email}", email);
            return StatusCode(500, new { message = "An error occurred while retrieving the user" });
        }
    }

    /// <summary>
    /// Get active users only
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active users</returns>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<UserSummaryDto>>> GetActiveUsers(CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _userRepository.GetActiveUsersAsync(cancellationToken);
            var userSummaries = users.Select(u => u.ToSummaryDto()).ToList();

            return Ok(userSummaries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active users");
            return StatusCode(500, new { message = "An error occurred while retrieving active users" });
        }
    }

    /// <summary>
    /// Get users by role
    /// </summary>
    /// <param name="roleName">Role name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of users in the specified role</returns>
    [HttpGet("by-role/{roleName}")]
    public async Task<ActionResult<IEnumerable<UserSummaryDto>>> GetUsersByRole(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _userRepository.GetUsersByRoleAsync(roleName, cancellationToken);
            var userSummaries = users.Select(u => u.ToSummaryDto()).ToList();

            return Ok(userSummaries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users by role {RoleName}", roleName);
            return StatusCode(500, new { message = "An error occurred while retrieving users by role" });
        }
    }

    /// <summary>
    /// Get user roles
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of user roles</returns>
    [HttpGet("{id:guid}/roles")]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetUserRoles(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // First check if user exists
            var userExists = await _userRepository.ExistsAsync(id, cancellationToken);
            if (!userExists)
            {
                return NotFound(new { message = "User not found" });
            }

            var roles = await _userRepository.GetUserRolesAsync(id, cancellationToken);
            var roleDtos = roles.Select(r => r.ToRoleDto()).ToList();

            return Ok(roleDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving roles for user {UserId}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving user roles" });
        }
    }

    /// <summary>
    /// Check if user is in role
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="roleName">Role name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Boolean indicating if user is in role</returns>
    [HttpGet("{id:guid}/is-in-role/{roleName}")]
    public async Task<ActionResult<bool>> IsInRole(Guid id, string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            var isInRole = await _userRepository.IsInRoleAsync(id, roleName, cancellationToken);
            return Ok(isInRole);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if user {UserId} is in role {RoleName}", id, roleName);
            return StatusCode(500, new { message = "An error occurred while checking user role" });
        }
    }
}