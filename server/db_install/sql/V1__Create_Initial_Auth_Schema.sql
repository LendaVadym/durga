-- =============================================
-- Durga Authentication Schema - Initial Migration
-- Version: 1.0
-- Description: Create core authentication tables (Users, Roles, UserRoles)
-- =============================================

-- Create Roles table
CREATE TABLE [dbo].[Roles] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [Name] NVARCHAR(256) NOT NULL,
    [NormalizedName] NVARCHAR(256) NULL,
    [Description] NVARCHAR(500) NULL,
    [IsSystemRole] BIT NOT NULL DEFAULT 0,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] NVARCHAR(256) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(256) NULL,
    [ConcurrencyStamp] NVARCHAR(MAX) NULL,
    
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

-- Create unique index on role name
CREATE UNIQUE NONCLUSTERED INDEX [IX_Roles_NormalizedName] 
ON [dbo].[Roles] ([NormalizedName]) 
WHERE [NormalizedName] IS NOT NULL;

-- Create Users table
CREATE TABLE [dbo].[Users] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [UserName] NVARCHAR(256) NOT NULL,
    [NormalizedUserName] NVARCHAR(256) NULL,
    [Email] NVARCHAR(256) NOT NULL,
    [NormalizedEmail] NVARCHAR(256) NULL,
    [EmailConfirmed] BIT NOT NULL DEFAULT 0,
    [PasswordHash] NVARCHAR(MAX) NULL,
    [SecurityStamp] NVARCHAR(MAX) NULL,
    [ConcurrencyStamp] NVARCHAR(MAX) NULL,
    [PhoneNumber] NVARCHAR(50) NULL,
    [PhoneNumberConfirmed] BIT NOT NULL DEFAULT 0,
    [TwoFactorEnabled] BIT NOT NULL DEFAULT 0,
    [LockoutEnd] DATETIMEOFFSET NULL,
    [LockoutEnabled] BIT NOT NULL DEFAULT 1,
    [AccessFailedCount] INT NOT NULL DEFAULT 0,
    
    -- Extended user properties
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [FullName] AS ([FirstName] + ' ' + [LastName]) PERSISTED,
    [DateOfBirth] DATE NULL,
    [Gender] NVARCHAR(10) NULL,
    [ProfileImageUrl] NVARCHAR(500) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [IsEmailVerified] BIT NOT NULL DEFAULT 0,
    [LastLoginAt] DATETIME2 NULL,
    [LastPasswordChangedAt] DATETIME2 NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] NVARCHAR(256) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(256) NULL,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] NVARCHAR(256) NULL,
    
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

-- Create indexes on Users table
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_NormalizedUserName] 
ON [dbo].[Users] ([NormalizedUserName]) 
WHERE [NormalizedUserName] IS NOT NULL;

CREATE UNIQUE NONCLUSTERED INDEX [IX_Users_NormalizedEmail] 
ON [dbo].[Users] ([NormalizedEmail]) 
WHERE [NormalizedEmail] IS NOT NULL;

CREATE NONCLUSTERED INDEX [IX_Users_Email] 
ON [dbo].[Users] ([Email]);

CREATE NONCLUSTERED INDEX [IX_Users_IsActive] 
ON [dbo].[Users] ([IsActive]);

CREATE NONCLUSTERED INDEX [IX_Users_CreatedAt] 
ON [dbo].[Users] ([CreatedAt]);

-- Create UserRoles junction table
CREATE TABLE [dbo].[UserRoles] (
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [RoleId] UNIQUEIDENTIFIER NOT NULL,
    [AssignedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [AssignedBy] NVARCHAR(256) NULL,
    [ExpiresAt] DATETIME2 NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([RoleId]) 
        REFERENCES [dbo].[Roles] ([Id]) ON DELETE CASCADE
);

-- Create indexes on UserRoles table
CREATE NONCLUSTERED INDEX [IX_UserRoles_UserId] 
ON [dbo].[UserRoles] ([UserId]);

CREATE NONCLUSTERED INDEX [IX_UserRoles_RoleId] 
ON [dbo].[UserRoles] ([RoleId]);

CREATE NONCLUSTERED INDEX [IX_UserRoles_IsActive] 
ON [dbo].[UserRoles] ([IsActive]);

-- Add comments to tables
EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Stores user role definitions with metadata',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'Roles';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Stores user account information with extended profile data',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'Users';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Junction table linking users to their assigned roles',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'UserRoles';