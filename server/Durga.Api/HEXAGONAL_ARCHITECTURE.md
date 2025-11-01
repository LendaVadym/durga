# Durga API - Clean Hexagonal Architecture Implementation

## Overview

This document describes the clean hexagonal architecture (ports and adapters) implementation for the Durga API. The system now follows proper clean architecture principles with clear separation between layers and includes entities for Users, Roles, Departments, and Teams.

## Architecture Structure

```
Durga.Api/
├── Domain/                          # Core Domain Layer (Inner Layer)
│   ├── Entities/                   # Domain Entities
│   │   ├── BaseEntity.cs          # Base entity with common properties
│   │   ├── User.cs                # User domain entity
│   │   ├── Role.cs                # Role domain entity
│   │   ├── UserRole.cs            # UserRole junction entity
│   │   ├── Department.cs          # Department domain entity
│   │   ├── Team.cs                # Team domain entity
│   │   └── TeamUser.cs            # TeamUser junction entity
│   └── ValueObjects/              # Domain Value Objects (future)
│
├── Application/                    # Application Layer
│   ├── Ports/                     # Interfaces (Ports)
│   │   ├── IUserRepository.cs     # User repository contract
│   │   ├── IRoleRepository.cs     # Role repository contract
│   │   ├── IDepartmentRepository.cs # Department repository contract
│   │   ├── ITeamRepository.cs     # Team repository contract
│   │   ├── IEmailService.cs       # Email service contract
│   │   └── IFileStorageService.cs # File storage service contract
│   ├── Services/                  # Application Services (Business Logic)
│   │   ├── IUserService.cs        # User service interface
│   │   ├── IDepartmentService.cs  # Department service interface
│   │   └── ITeamService.cs        # Team service interface
│   ├── DTOs/                      # Data Transfer Objects
│   │   ├── Users/                 # User-related DTOs
│   │   │   └── UserDtos.cs       # User DTOs
│   │   ├── Roles/                 # Role-related DTOs
│   │   │   └── RoleDtos.cs       # Role DTOs
│   │   ├── Departments/           # Department-related DTOs
│   │   │   └── DepartmentDtos.cs # Department DTOs
│   │   ├── Teams/                 # Team-related DTOs
│   │   │   └── TeamDtos.cs       # Team DTOs
│   │   ├── AuthDtos.cs           # Authentication DTOs
│   │   └── MappingExtensions.cs  # Domain to DTO mapping
│   ├── UseCases/                  # Use Cases (Command/Query handlers)
│   └── Common/                    # Common application logic
│
├── Infrastructure/                 # Infrastructure Layer (Outer Layer)
│   ├── Adapters/                  # Concrete Implementations (Adapters)
│   │   ├── Persistence/           # Database Context
│   │   │   └── DurgaDbContext.cs  # EF Core DbContext for PostgreSQL
│   │   └── Repositories/          # Repository Implementations
│   │       ├── UserRepository.cs  # User repository implementation
│   │       ├── RoleRepository.cs  # Role repository implementation
│   │       ├── DepartmentRepository.cs # Department repository implementation
│   │       └── TeamRepository.cs  # Team repository implementation
│   ├── Identity/                  # ASP.NET Identity Infrastructure
│   │   ├── ApplicationUser.cs     # Identity user model
│   │   └── ApplicationDbContext.cs # Identity DbContext
│   └── External/                  # External Service Adapters
│       ├── EmailService.cs        # Email service implementation
│       └── FileStorageService.cs  # File storage service implementation
│
└── Presentation/                   # Presentation Layer (Outer Layer)
    └── Controllers/               # Web API Controllers
        ├── UsersController.cs    # User management endpoints
        ├── RolesController.cs    # Role management endpoints
        ├── DepartmentsController.cs # Department management endpoints
        ├── TeamsController.cs    # Team management endpoints
        └── AuthController.cs     # Authentication endpoints
```

## Key Components

### 1. Domain Layer

**Entities:**
- `BaseEntity`: Common properties for all entities (Id, CreatedAt, etc.)
- `User`: Complete user entity with authentication and profile data
- `Role`: Role entity with system/user-defined role support
- `UserRole`: Junction entity for many-to-many user-role relationships
- `Department`: Department entity with manager relationships
- `Team`: Team entity with department, leader, and manager relationships
- `TeamUser`: Junction entity for team membership management

**Domain Logic:**
- Business rules are encapsulated in entities (e.g., `IsDeleted`, `CanLogin`, `IsValidAssignment`)
- Rich domain models with behavior, not just data containers
- Team membership logic with leader and manager roles
- Department hierarchy management

### 2. Application Layer

**Ports (Interfaces):**
- `IUserRepository`: Defines all user data operations
- `IRoleRepository`: Defines all role data operations
- `IDepartmentRepository`: Defines all department data operations
- `ITeamRepository`: Defines all team data operations
- `IEmailService`: Email communication service contract
- `IFileStorageService`: File storage service contract
- Clean contracts independent of infrastructure concerns

**Application Services:**
- `IUserService`: Business logic for user management
- `IDepartmentService`: Business logic for department operations
- `ITeamService`: Business logic for team management
- Orchestrate domain entities and repository operations
- Handle cross-cutting concerns and business workflows

### 3. Infrastructure Layer

**Adapters:**
- `DurgaDbContext`: PostgreSQL-specific EF Core context
- `UserRepository`: PostgreSQL implementation of `IUserRepository`
- `RoleRepository`: PostgreSQL implementation of `IRoleRepository`

**Features:**
- Complete EF Core configuration with proper column mapping
- PostgreSQL-specific data types and conventions
- Optimized queries with proper includes and filtering

### 4. Web Layer

**Controllers:**
- `UsersController`: RESTful API for user operations
- `RolesController`: RESTful API for role operations
- Proper error handling and logging
- Clean separation using DTOs

## Benefits of This Architecture

### 1. **Separation of Concerns**
- Domain logic is independent of infrastructure
- Business rules are centralized in entities
- Data access is abstracted through ports

### 2. **Testability**
- Easy to mock repositories for unit testing
- Domain logic can be tested without database
- Clear boundaries between layers

### 3. **Flexibility**
- Easy to swap database providers (PostgreSQL → MySQL → MongoDB)
- Multiple implementations of repositories (caching, read replicas)
- Independent evolution of layers

### 4. **Maintainability**
- Clear structure and responsibilities
- Easy to locate and modify functionality
- Follows SOLID principles

## Database Schema Mapping

The implementation maps to the existing PostgreSQL schema:

- **users** table → `User` entity
- **roles** table → `Role` entity  
- **user_roles** table → `UserRole` entity

### Key Mappings:
- PostgreSQL snake_case → C# PascalCase
- UUID → Guid
- TIMESTAMP WITH TIME ZONE → DateTime
- BOOLEAN → bool

## API Endpoints

### Users API (`/api/users`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users` | Get users with pagination and search |
| GET | `/api/users/{id}` | Get user by ID |
| GET | `/api/users/by-email/{email}` | Get user by email |
| GET | `/api/users/active` | Get active users only |
| GET | `/api/users/by-role/{roleName}` | Get users by role |
| GET | `/api/users/{id}/roles` | Get user's roles |
| GET | `/api/users/{id}/is-in-role/{roleName}` | Check if user has role |

### Roles API (`/api/roles`)

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/roles` | Get roles with pagination and search |
| GET | `/api/roles/{id}` | Get role by ID |
| GET | `/api/roles/by-name/{name}` | Get role by name |
| GET | `/api/roles/active` | Get active roles only |
| GET | `/api/roles/system` | Get system roles |
| GET | `/api/roles/user-defined` | Get user-defined roles |
| GET | `/api/roles/{id}/users` | Get users in role |
| GET | `/api/roles/{id}/user-count` | Get user count in role |

## Configuration

### Dependencies
```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.11" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.9" />
```

### Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=postgres;Port=5432;Database=DurgaDb;Username=durga_user;Password=DurgaServer2024!"
  }
}
```

### Dependency Injection
```csharp
// Hexagonal Architecture DbContext
builder.Services.AddDbContext<DurgaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository pattern - Hexagonal Architecture
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
```

## Usage Examples

### Getting Users with Pagination
```csharp
var pagedResult = await _userRepository.GetPagedAsync(
    pageNumber: 1, 
    pageSize: 10, 
    searchTerm: "john", 
    includeInactive: false);

var userListDto = pagedResult.ToListDto(1, 10);
```

### Checking User Roles
```csharp
var isAdmin = await _userRepository.IsInRoleAsync(userId, "Admin");
var userRoles = await _userRepository.GetUserRolesAsync(userId);
```

### Getting Role Statistics
```csharp
var adminUsers = await _roleRepository.GetRoleUsersAsync("Admin");
var userCount = await _roleRepository.GetUserCountInRoleAsync(roleId);
```

## Future Enhancements

1. **CQRS Implementation**: Separate read/write models
2. **Domain Events**: Event-driven architecture
3. **Caching Layer**: Redis caching adapter
4. **Audit Trail**: Track all data changes
5. **Read Replicas**: Separate read/write database connections

## Testing Strategy

1. **Unit Tests**: Test domain entities and business logic
2. **Integration Tests**: Test repositories against test database
3. **API Tests**: Test controllers with mocked repositories
4. **End-to-End Tests**: Test complete flows

This implementation provides a solid foundation for scalable, maintainable user and role management following hexagonal architecture principles.