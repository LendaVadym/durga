namespace Durga.Api.Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid? ManagerId { get; set; }

    // Navigation properties
    public virtual User? Manager { get; set; }
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();

    // Domain methods
    public bool HasManager => ManagerId.HasValue && Manager != null;
}