using Microsoft.EntityFrameworkCore;
using Durga.Api.Domain.Entities;

namespace Durga.Api.Infrastructure.Adapters.Persistence;

public class DurgaDbContext : DbContext
{
    public DurgaDbContext(DbContextOptions<DurgaDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Team> Teams { get; set; } = null!;
    public DbSet<TeamUser> TeamUsers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUserEntity(modelBuilder);
        ConfigureRoleEntity(modelBuilder);
        ConfigureUserRoleEntity(modelBuilder);
        ConfigureDepartmentEntity(modelBuilder);
        ConfigureTeamEntity(modelBuilder);
        ConfigureTeamUserEntity(modelBuilder);
    }

    private static void ConfigureUserEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<User>();
        
        // Table mapping
        entity.ToTable("users");
        
        // Column mappings
        entity.Property(u => u.Id).HasColumnName("id");
        entity.Property(u => u.UserName).HasColumnName("user_name");
        entity.Property(u => u.NormalizedUserName).HasColumnName("normalized_user_name");
        entity.Property(u => u.Email).HasColumnName("email");
        entity.Property(u => u.NormalizedEmail).HasColumnName("normalized_email");
        entity.Property(u => u.EmailConfirmed).HasColumnName("email_confirmed");
        entity.Property(u => u.PasswordHash).HasColumnName("password_hash");
        entity.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
        entity.Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp");
        entity.Property(u => u.PhoneNumber).HasColumnName("phone_number");
        entity.Property(u => u.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
        entity.Property(u => u.TwoFactorEnabled).HasColumnName("two_factor_enabled");
        entity.Property(u => u.LockoutEnd).HasColumnName("lockout_end");
        entity.Property(u => u.LockoutEnabled).HasColumnName("lockout_enabled");
        entity.Property(u => u.AccessFailedCount).HasColumnName("access_failed_count");
        entity.Property(u => u.FirstName).HasColumnName("first_name");
        entity.Property(u => u.LastName).HasColumnName("last_name");
        entity.Property(u => u.DateOfBirth).HasColumnName("date_of_birth");
        entity.Property(u => u.Gender).HasColumnName("gender");
        entity.Property(u => u.ProfileImageUrl).HasColumnName("profile_image_url");
        entity.Property(u => u.IsActive).HasColumnName("is_active");
        entity.Property(u => u.IsEmailVerified).HasColumnName("is_email_verified");
        entity.Property(u => u.LastLoginAt).HasColumnName("last_login_at");
        entity.Property(u => u.LastPasswordChangedAt).HasColumnName("last_password_changed_at");
        entity.Property(u => u.CreatedAt).HasColumnName("created_at");
        entity.Property(u => u.CreatedBy).HasColumnName("created_by");
        entity.Property(u => u.UpdatedAt).HasColumnName("updated_at");
        entity.Property(u => u.UpdatedBy).HasColumnName("updated_by");
        entity.Property(u => u.DeletedAt).HasColumnName("deleted_at");
        entity.Property(u => u.DeletedBy).HasColumnName("deleted_by");
        
        // Computed property - ignored since it's calculated in code
        entity.Ignore(u => u.FullName);
    }

    private static void ConfigureRoleEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Role>();
        
        // Table mapping
        entity.ToTable("roles");
        
        // Column mappings
        entity.Property(r => r.Id).HasColumnName("id");
        entity.Property(r => r.Name).HasColumnName("name");
        entity.Property(r => r.NormalizedName).HasColumnName("normalized_name");
        entity.Property(r => r.Description).HasColumnName("description");
        entity.Property(r => r.ConcurrencyStamp).HasColumnName("concurrency_stamp");
        entity.Property(r => r.IsSystemRole).HasColumnName("is_system_role");
        entity.Property(r => r.IsActive).HasColumnName("is_active");
        entity.Property(r => r.CreatedAt).HasColumnName("created_at");
        entity.Property(r => r.CreatedBy).HasColumnName("created_by");
        entity.Property(r => r.UpdatedAt).HasColumnName("updated_at");
        entity.Property(r => r.UpdatedBy).HasColumnName("updated_by");
    }

    private static void ConfigureUserRoleEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<UserRole>();
        
        // Table mapping
        entity.ToTable("user_roles");
        
        // Composite primary key
        entity.HasKey(ur => new { ur.UserId, ur.RoleId });
        
        // Column mappings
        entity.Property(ur => ur.UserId).HasColumnName("user_id");
        entity.Property(ur => ur.RoleId).HasColumnName("role_id");
        entity.Property(ur => ur.AssignedAt).HasColumnName("assigned_at");
        entity.Property(ur => ur.AssignedBy).HasColumnName("assigned_by");
        entity.Property(ur => ur.ExpiresAt).HasColumnName("expires_at");
        entity.Property(ur => ur.IsActive).HasColumnName("is_active");
        
        // Relationships
        entity.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
            
        entity.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }

    private static void ConfigureDepartmentEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Department>();
        
        // Table mapping
        entity.ToTable("departments");
        
        // Column mappings
        entity.Property(d => d.Id).HasColumnName("id");
        entity.Property(d => d.Name).HasColumnName("name");
        entity.Property(d => d.ManagerId).HasColumnName("manager_id");
        entity.Property(d => d.CreatedAt).HasColumnName("created_at");
        entity.Property(d => d.CreatedBy).HasColumnName("created_by");
        entity.Property(d => d.UpdatedAt).HasColumnName("updated_at");
        entity.Property(d => d.UpdatedBy).HasColumnName("updated_by");
        
        // Relationships
        entity.HasOne(d => d.Manager)
            .WithMany(u => u.ManagedDepartments)
            .HasForeignKey(d => d.ManagerId);
    }

    private static void ConfigureTeamEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<Team>();
        
        // Table mapping
        entity.ToTable("teams");
        
        // Column mappings
        entity.Property(t => t.Id).HasColumnName("id");
        entity.Property(t => t.Name).HasColumnName("name");
        entity.Property(t => t.Description).HasColumnName("description");
        entity.Property(t => t.DepartmentId).HasColumnName("department_id");
        entity.Property(t => t.LeaderId).HasColumnName("leader_id");
        entity.Property(t => t.ManagerId).HasColumnName("manager_id");
        entity.Property(t => t.IsActive).HasColumnName("is_active");
        entity.Property(t => t.CreatedAt).HasColumnName("created_at");
        entity.Property(t => t.CreatedBy).HasColumnName("created_by");
        entity.Property(t => t.UpdatedAt).HasColumnName("updated_at");
        entity.Property(t => t.UpdatedBy).HasColumnName("updated_by");
        
        // Relationships
        entity.HasOne(t => t.Department)
            .WithMany(d => d.Teams)
            .HasForeignKey(t => t.DepartmentId);
            
        entity.HasOne(t => t.Leader)
            .WithMany(u => u.LeadTeams)
            .HasForeignKey(t => t.LeaderId);
            
        entity.HasOne(t => t.Manager)
            .WithMany(u => u.ManagedTeams)
            .HasForeignKey(t => t.ManagerId);
    }

    private static void ConfigureTeamUserEntity(ModelBuilder modelBuilder)
    {
        var entity = modelBuilder.Entity<TeamUser>();
        
        // Table mapping
        entity.ToTable("team_users");
        
        // Composite primary key
        entity.HasKey(tu => new { tu.TeamId, tu.UserId });
        
        // Column mappings
        entity.Property(tu => tu.TeamId).HasColumnName("team_id");
        entity.Property(tu => tu.UserId).HasColumnName("user_id");
        entity.Property(tu => tu.JoinedAt).HasColumnName("joined_at");
        entity.Property(tu => tu.JoinedBy).HasColumnName("joined_by");
        entity.Property(tu => tu.LeftAt).HasColumnName("left_at");
        entity.Property(tu => tu.LeftBy).HasColumnName("left_by");
        entity.Property(tu => tu.IsActive).HasColumnName("is_active");
        
        // Relationships
        entity.HasOne(tu => tu.Team)
            .WithMany(t => t.TeamUsers)
            .HasForeignKey(tu => tu.TeamId);
            
        entity.HasOne(tu => tu.User)
            .WithMany(u => u.TeamUsers)
            .HasForeignKey(tu => tu.UserId);
    }
}
