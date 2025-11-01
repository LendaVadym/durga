namespace Durga.Api.Domain.Entities;

public class User : BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string? NormalizedUserName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; } = false;
    public string? PasswordHash { get; set; }
    public string? SecurityStamp { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; } = false;
    public bool TwoFactorEnabled { get; set; } = false;
    public DateTimeOffset? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; } = true;
    public int AccessFailedCount { get; set; } = 0;

    // Extended user properties
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public DateOnly? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? ProfileImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsEmailVerified { get; set; } = false;
    public DateTime? LastLoginAt { get; set; }
    public DateTime? LastPasswordChangedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    // Navigation properties
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<Role> Roles => UserRoles.Select(ur => ur.Role).ToList();
    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
    public virtual ICollection<Team> Teams => TeamUsers.Where(tu => tu.IsCurrentMember).Select(tu => tu.Team).ToList();
    public virtual ICollection<Department> ManagedDepartments { get; set; } = new List<Department>();
    public virtual ICollection<Team> LeadTeams { get; set; } = new List<Team>();
    public virtual ICollection<Team> ManagedTeams { get; set; } = new List<Team>();

    // Domain methods
    public bool IsDeleted => DeletedAt.HasValue;
    public bool CanLogin => IsActive && !IsDeleted && EmailConfirmed;
}