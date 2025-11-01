namespace Durga.Api.Domain.Entities;

public class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid? LeaderId { get; set; }
    public Guid? ManagerId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Department Department { get; set; } = null!;
    public virtual User? Leader { get; set; }
    public virtual User? Manager { get; set; }
    public virtual ICollection<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();
    public virtual ICollection<User> Users => TeamUsers.Where(tu => tu.IsCurrentMember).Select(tu => tu.User).ToList();

    // Domain methods
    public bool HasLeader => LeaderId.HasValue && Leader != null;
    public bool HasManager => ManagerId.HasValue && Manager != null;
    public bool IsLeaderInTeam => LeaderId.HasValue && TeamUsers.Any(tu => tu.UserId == LeaderId.Value && tu.IsCurrentMember);
    public bool IsManagerInTeam => ManagerId.HasValue && TeamUsers.Any(tu => tu.UserId == ManagerId.Value && tu.IsCurrentMember);
    public int ActiveMemberCount => TeamUsers.Count(tu => tu.IsCurrentMember);
}