-- =============================================
-- Durga Authentication Schema - Extended Tables
-- Version: 2.0
-- Description: Create additional auth tables (Claims, Tokens, Login attempts, etc.)
-- =============================================

-- Create RoleClaims table for role-based permissions
CREATE TABLE [dbo].[RoleClaims] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [RoleId] UNIQUEIDENTIFIER NOT NULL,
    [ClaimType] NVARCHAR(256) NULL,
    [ClaimValue] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] NVARCHAR(256) NULL,
    
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RoleClaims_Roles] FOREIGN KEY ([RoleId]) 
        REFERENCES [dbo].[Roles] ([Id]) ON DELETE CASCADE
);

-- Create UserClaims table for user-specific permissions
CREATE TABLE [dbo].[UserClaims] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [ClaimType] NVARCHAR(256) NULL,
    [ClaimValue] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [CreatedBy] NVARCHAR(256) NULL,
    
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserClaims_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

-- Create UserLogins table for external login providers
CREATE TABLE [dbo].[UserLogins] (
    [LoginProvider] NVARCHAR(128) NOT NULL,
    [ProviderKey] NVARCHAR(128) NOT NULL,
    [ProviderDisplayName] NVARCHAR(256) NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [LastUsedAt] DATETIME2 NULL,
    
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_UserLogins_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

-- Create UserTokens table for storing user tokens (refresh tokens, etc.)
CREATE TABLE [dbo].[UserTokens] (
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [LoginProvider] NVARCHAR(128) NOT NULL,
    [Name] NVARCHAR(128) NOT NULL,
    [Value] NVARCHAR(MAX) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ExpiresAt] DATETIME2 NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

-- Create RefreshTokens table for JWT refresh token management
CREATE TABLE [dbo].[RefreshTokens] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Token] NVARCHAR(512) NOT NULL,
    [JwtId] NVARCHAR(256) NOT NULL,
    [IsUsed] BIT NOT NULL DEFAULT 0,
    [IsRevoked] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ExpiresAt] DATETIME2 NOT NULL,
    [RevokedAt] DATETIME2 NULL,
    [RevokedReason] NVARCHAR(256) NULL,
    [ReplacedByToken] NVARCHAR(512) NULL,
    [ClientId] NVARCHAR(128) NULL,
    [IpAddress] NVARCHAR(45) NULL,
    [UserAgent] NVARCHAR(512) NULL,
    
    CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

-- Create UserSessions table to track active sessions
CREATE TABLE [dbo].[UserSessions] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [SessionToken] NVARCHAR(512) NOT NULL,
    [DeviceInfo] NVARCHAR(512) NULL,
    [IpAddress] NVARCHAR(45) NULL,
    [UserAgent] NVARCHAR(512) NULL,
    [Location] NVARCHAR(256) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ExpiresAt] DATETIME2 NOT NULL,
    [LastAccessedAt] DATETIME2 NULL,
    [EndedAt] DATETIME2 NULL,
    [EndReason] NVARCHAR(128) NULL,
    
    CONSTRAINT [PK_UserSessions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserSessions_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

-- Create LoginAttempts table for security monitoring
CREATE TABLE [dbo].[LoginAttempts] (
    [Id] BIGINT IDENTITY(1,1) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL,
    [UserId] UNIQUEIDENTIFIER NULL,
    [IpAddress] NVARCHAR(45) NOT NULL,
    [UserAgent] NVARCHAR(512) NULL,
    [IsSuccessful] BIT NOT NULL,
    [FailureReason] NVARCHAR(256) NULL,
    [AttemptedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [Location] NVARCHAR(256) NULL,
    [DeviceInfo] NVARCHAR(512) NULL,
    
    CONSTRAINT [PK_LoginAttempts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_LoginAttempts_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) ON DELETE SET NULL
);

-- Create PasswordResetTokens table
CREATE TABLE [dbo].[PasswordResetTokens] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Token] NVARCHAR(512) NOT NULL,
    [IsUsed] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ExpiresAt] DATETIME2 NOT NULL,
    [UsedAt] DATETIME2 NULL,
    [IpAddress] NVARCHAR(45) NULL,
    
    CONSTRAINT [PK_PasswordResetTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PasswordResetTokens_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

-- Create EmailVerificationTokens table
CREATE TABLE [dbo].[EmailVerificationTokens] (
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Token] NVARCHAR(512) NOT NULL,
    [Email] NVARCHAR(256) NOT NULL,
    [IsUsed] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ExpiresAt] DATETIME2 NOT NULL,
    [VerifiedAt] DATETIME2 NULL,
    
    CONSTRAINT [PK_EmailVerificationTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_EmailVerificationTokens_Users] FOREIGN KEY ([UserId]) 
        REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

-- Create indexes for performance
CREATE NONCLUSTERED INDEX [IX_RoleClaims_RoleId] 
ON [dbo].[RoleClaims] ([RoleId]);

CREATE NONCLUSTERED INDEX [IX_UserClaims_UserId] 
ON [dbo].[UserClaims] ([UserId]);

CREATE NONCLUSTERED INDEX [IX_UserLogins_UserId] 
ON [dbo].[UserLogins] ([UserId]);

CREATE NONCLUSTERED INDEX [IX_UserTokens_UserId] 
ON [dbo].[UserTokens] ([UserId]);

CREATE UNIQUE NONCLUSTERED INDEX [IX_RefreshTokens_Token] 
ON [dbo].[RefreshTokens] ([Token]);

CREATE NONCLUSTERED INDEX [IX_RefreshTokens_UserId] 
ON [dbo].[RefreshTokens] ([UserId]);

CREATE NONCLUSTERED INDEX [IX_RefreshTokens_ExpiresAt] 
ON [dbo].[RefreshTokens] ([ExpiresAt]);

CREATE NONCLUSTERED INDEX [IX_UserSessions_UserId] 
ON [dbo].[UserSessions] ([UserId]);

CREATE NONCLUSTERED INDEX [IX_UserSessions_IsActive] 
ON [dbo].[UserSessions] ([IsActive]);

CREATE UNIQUE NONCLUSTERED INDEX [IX_UserSessions_SessionToken] 
ON [dbo].[UserSessions] ([SessionToken]);

CREATE NONCLUSTERED INDEX [IX_LoginAttempts_Email] 
ON [dbo].[LoginAttempts] ([Email]);

CREATE NONCLUSTERED INDEX [IX_LoginAttempts_IpAddress] 
ON [dbo].[LoginAttempts] ([IpAddress]);

CREATE NONCLUSTERED INDEX [IX_LoginAttempts_AttemptedAt] 
ON [dbo].[LoginAttempts] ([AttemptedAt]);

CREATE NONCLUSTERED INDEX [IX_PasswordResetTokens_UserId] 
ON [dbo].[PasswordResetTokens] ([UserId]);

CREATE UNIQUE NONCLUSTERED INDEX [IX_PasswordResetTokens_Token] 
ON [dbo].[PasswordResetTokens] ([Token]);

CREATE NONCLUSTERED INDEX [IX_EmailVerificationTokens_UserId] 
ON [dbo].[EmailVerificationTokens] ([UserId]);

CREATE UNIQUE NONCLUSTERED INDEX [IX_EmailVerificationTokens_Token] 
ON [dbo].[EmailVerificationTokens] ([Token]);

-- Add table descriptions
EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Stores claims/permissions associated with roles',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'RoleClaims';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Stores claims/permissions specific to individual users',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'UserClaims';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Stores external login provider associations',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'UserLogins';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Stores various user tokens and their metadata',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'UserTokens';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Manages JWT refresh tokens with security tracking',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'RefreshTokens';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Tracks active user sessions across devices',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'UserSessions';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Logs all login attempts for security monitoring',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'LoginAttempts';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Manages password reset tokens with expiration',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'PasswordResetTokens';

EXEC sp_addextendedproperty 
    @name = N'MS_Description',
    @value = N'Manages email verification tokens',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'EmailVerificationTokens';