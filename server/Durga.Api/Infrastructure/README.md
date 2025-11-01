# Infrastructure Layer - Context Separation Strategy

## Overview

This project uses a **dual-context approach** to separate concerns between Identity and Domain logic:

## Context Separation

### 1. ApplicationDbContext (Identity Infrastructure)
- **Location**: `Infrastructure/Identity/ApplicationDbContext.cs`
- **Purpose**: Handles ASP.NET Identity tables and authentication
- **Entities**: 
  - `ApplicationUser` (extends IdentityUser)
  - ASP.NET Identity tables (AspNetUsers, AspNetRoles, etc.)
- **Database**: Uses Identity schema with standard ASP.NET Identity conventions

### 2. DurgaDbContext (Domain Infrastructure)
- **Location**: `Infrastructure/Adapters/Persistence/DurgaDbContext.cs`
- **Purpose**: Handles domain entities and business logic
- **Entities**: 
  - `User`, `Role`, `UserRole` (domain entities)
  - `Department`, `Team`, `TeamUser` (business entities)
- **Database**: Uses custom domain schema with snake_case PostgreSQL conventions

## Why Dual Context?

### ✅ Benefits
1. **Separation of Concerns**: Identity logic is isolated from domain logic
2. **Independent Evolution**: Identity and domain schemas can evolve separately
3. **Security**: Identity data has different security requirements than business data
4. **Flexibility**: Can use different databases or configurations for each context
5. **Clean Architecture**: Follows the principle of having separate contexts for different bounded contexts

### 🔧 Implementation Details

**Connection Strings**: Both contexts use the same connection string but manage different table sets:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=durga_db;Username=postgres;Password=password"
  }
}
```

**Dependency Injection**: Both contexts are registered separately:
```csharp
// Identity Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Domain Context
builder.Services.AddDbContext<DurgaDbContext>(options =>
    options.UseNpgsql(connectionString));
```

## Schema Management

- **Identity Schema**: Managed by ASP.NET Identity migrations (if needed)
- **Domain Schema**: Managed by Flyway migrations in `/db_install/sql/`

## Data Synchronization

When a user is created in the Identity system, a corresponding domain User entity should be created to maintain referential integrity. This is handled in the application services layer.

## Future Considerations

- Consider implementing a **User Synchronization Service** to keep Identity and Domain users in sync
- Potential to use **different databases** for Identity and Domain contexts if scaling requires it
- **Event-driven synchronization** between contexts using domain events