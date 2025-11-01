namespace Durga.Api.Application.DTOs.Departments;

public record DepartmentDto(
    Guid Id,
    string Name,
    Guid? ManagerId,
    string? ManagerName,
    DateTime CreatedAt,
    string? CreatedBy,
    DateTime? UpdatedAt,
    string? UpdatedBy,
    int TeamCount
);

public record CreateDepartmentDto(
    string Name,
    Guid? ManagerId
);

public record UpdateDepartmentDto(
    string Name,
    Guid? ManagerId
);

public record DepartmentListItemDto(
    Guid Id,
    string Name,
    string? ManagerName,
    int TeamCount,
    DateTime CreatedAt
);

public record DepartmentSummaryDto(
    Guid Id,
    string Name,
    string? ManagerName
);