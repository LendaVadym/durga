namespace Durga.Api.Domain.Entities;

public class TeamUser
{
    public Guid TeamId { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public string? JoinedBy { get; set; }
    public DateTime? LeftAt { get; set; }
    public string? LeftBy { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Team Team { get; set; } = null!;
    public virtual User User { get; set; } = null!;

    // Domain methods
    public bool IsCurrentMember => IsActive && !LeftAt.HasValue;
    public bool HasLeft => LeftAt.HasValue;
}