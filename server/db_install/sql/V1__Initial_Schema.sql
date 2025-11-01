-- ================================================================================================
-- Durga Application Database Schema - Initial Version
-- Flyway Migration: V1__Initial_Schema.sql
-- Description: Creates all necessary tables, indexes, and constraints for the Durga application
-- ================================================================================================

-- Enable UUID extension for PostgreSQL
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- ================================================================================================
-- ASP.NET IDENTITY TABLES
-- ================================================================================================

-- AspNetRoles table (Identity roles)
CREATE TABLE IF NOT EXISTS "AspNetRoles" (
    "Id" varchar(450) NOT NULL,
    "Name" varchar(256) NULL,
    "NormalizedName" varchar(256) NULL,
    "ConcurrencyStamp" text NULL,
    CONSTRAINT "PK_AspNetRoles" PRIMARY KEY ("Id")
);

-- AspNetUsers table (Identity users - ApplicationUser)
CREATE TABLE IF NOT EXISTS "AspNetUsers" (
    "Id" varchar(450) NOT NULL,
    "UserName" varchar(256) NULL,
    "NormalizedUserName" varchar(256) NULL,
    "Email" varchar(256) NULL,
    "NormalizedEmail" varchar(256) NULL,
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text NULL,
    "SecurityStamp" text NULL,
    "ConcurrencyStamp" text NULL,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamptz NULL,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    -- ApplicationUser extended properties
    "FirstName" varchar(100) NULL,
    "LastName" varchar(100) NULL,
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT NOW(),
    "LastLoginAt" timestamp with time zone NULL,
    "IsActive" boolean NOT NULL DEFAULT true,
    CONSTRAINT "PK_AspNetUsers" PRIMARY KEY ("Id")
);

-- AspNetRoleClaims table
CREATE TABLE IF NOT EXISTS "AspNetRoleClaims" (
    "Id" serial NOT NULL,
    "RoleId" varchar(450) NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

-- AspNetUserClaims table
CREATE TABLE IF NOT EXISTS "AspNetUserClaims" (
    "Id" serial NOT NULL,
    "UserId" varchar(450) NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- AspNetUserLogins table
CREATE TABLE IF NOT EXISTS "AspNetUserLogins" (
    "LoginProvider" varchar(450) NOT NULL,
    "ProviderKey" varchar(450) NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" varchar(450) NOT NULL,
    CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- AspNetUserRoles table
CREATE TABLE IF NOT EXISTS "AspNetUserRoles" (
    "UserId" varchar(450) NOT NULL,
    "RoleId" varchar(450) NOT NULL,
    CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- AspNetUserTokens table
CREATE TABLE IF NOT EXISTS "AspNetUserTokens" (
    "UserId" varchar(450) NOT NULL,
    "LoginProvider" varchar(450) NOT NULL,
    "Name" varchar(450) NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- ================================================================================================
-- CUSTOM DURGA DOMAIN TABLES
-- ================================================================================================

-- Users table (Custom domain users)
CREATE TABLE IF NOT EXISTS "users" (
    "id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "user_name" varchar(256) NOT NULL,
    "normalized_user_name" varchar(256) NULL,
    "email" varchar(256) NOT NULL,
    "normalized_email" varchar(256) NULL,
    "email_confirmed" boolean NOT NULL DEFAULT false,
    "password_hash" text NULL,
    "security_stamp" text NULL,
    "concurrency_stamp" text NULL,
    "phone_number" varchar(50) NULL,
    "phone_number_confirmed" boolean NOT NULL DEFAULT false,
    "two_factor_enabled" boolean NOT NULL DEFAULT false,
    "lockout_end" timestamptz NULL,
    "lockout_enabled" boolean NOT NULL DEFAULT true,
    "access_failed_count" integer NOT NULL DEFAULT 0,
    "first_name" varchar(100) NOT NULL,
    "last_name" varchar(100) NOT NULL,
    "date_of_birth" date NULL,
    "gender" varchar(10) NULL,
    "profile_image_url" varchar(500) NULL,
    "is_active" boolean NOT NULL DEFAULT true,
    "is_email_verified" boolean NOT NULL DEFAULT false,
    "last_login_at" timestamp with time zone NULL,
    "last_password_changed_at" timestamp with time zone NULL,
    "created_at" timestamp with time zone NOT NULL DEFAULT NOW(),
    "created_by" varchar(256) NULL,
    "updated_at" timestamp with time zone NULL,
    "updated_by" varchar(256) NULL,
    "deleted_at" timestamp with time zone NULL,
    "deleted_by" varchar(256) NULL,
    CONSTRAINT "PK_users" PRIMARY KEY ("id")
);

-- Roles table (Custom domain roles)
CREATE TABLE IF NOT EXISTS "roles" (
    "id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "name" varchar(256) NOT NULL,
    "normalized_name" varchar(256) NULL,
    "description" varchar(500) NULL,
    "concurrency_stamp" text NULL,
    "is_system_role" boolean NOT NULL DEFAULT false,
    "is_active" boolean NOT NULL DEFAULT true,
    "created_at" timestamp with time zone NOT NULL DEFAULT NOW(),
    "created_by" varchar(256) NULL,
    "updated_at" timestamp with time zone NULL,
    "updated_by" varchar(256) NULL,
    CONSTRAINT "PK_roles" PRIMARY KEY ("id")
);

-- UserRoles table (Custom domain user-role relationships)
CREATE TABLE IF NOT EXISTS "user_roles" (
    "user_id" uuid NOT NULL,
    "role_id" uuid NOT NULL,
    "assigned_at" timestamp with time zone NOT NULL DEFAULT NOW(),
    "assigned_by" varchar(256) NULL,
    "expires_at" timestamp with time zone NULL,
    "is_active" boolean NOT NULL DEFAULT true,
    CONSTRAINT "PK_user_roles" PRIMARY KEY ("user_id", "role_id"),
    CONSTRAINT "FK_user_roles_users_user_id" FOREIGN KEY ("user_id") REFERENCES "users" ("id") ON DELETE CASCADE,
    CONSTRAINT "FK_user_roles_roles_role_id" FOREIGN KEY ("role_id") REFERENCES "roles" ("id") ON DELETE CASCADE
);

-- Departments table
CREATE TABLE IF NOT EXISTS "departments" (
    "id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "name" varchar(256) NOT NULL,
    "manager_id" uuid NULL,
    "created_at" timestamp with time zone NOT NULL DEFAULT NOW(),
    "created_by" varchar(256) NULL,
    "updated_at" timestamp with time zone NULL,
    "updated_by" varchar(256) NULL,
    CONSTRAINT "PK_departments" PRIMARY KEY ("id"),
    CONSTRAINT "FK_departments_users_manager_id" FOREIGN KEY ("manager_id") REFERENCES "users" ("id") ON DELETE SET NULL
);

-- Teams table
CREATE TABLE IF NOT EXISTS "teams" (
    "id" uuid NOT NULL DEFAULT uuid_generate_v4(),
    "name" varchar(256) NOT NULL,
    "description" varchar(500) NULL,
    "department_id" uuid NOT NULL,
    "leader_id" uuid NULL,
    "manager_id" uuid NULL,
    "is_active" boolean NOT NULL DEFAULT true,
    "created_at" timestamp with time zone NOT NULL DEFAULT NOW(),
    "created_by" varchar(256) NULL,
    "updated_at" timestamp with time zone NULL,
    "updated_by" varchar(256) NULL,
    CONSTRAINT "PK_teams" PRIMARY KEY ("id"),
    CONSTRAINT "FK_teams_departments_department_id" FOREIGN KEY ("department_id") REFERENCES "departments" ("id") ON DELETE RESTRICT,
    CONSTRAINT "FK_teams_users_leader_id" FOREIGN KEY ("leader_id") REFERENCES "users" ("id") ON DELETE SET NULL,
    CONSTRAINT "FK_teams_users_manager_id" FOREIGN KEY ("manager_id") REFERENCES "users" ("id") ON DELETE SET NULL
);

-- TeamUsers table (Team-User many-to-many relationship)
CREATE TABLE IF NOT EXISTS "team_users" (
    "team_id" uuid NOT NULL,
    "user_id" uuid NOT NULL,
    "joined_at" timestamp with time zone NOT NULL DEFAULT NOW(),
    "joined_by" varchar(256) NULL,
    "left_at" timestamp with time zone NULL,
    "left_by" varchar(256) NULL,
    "is_active" boolean NOT NULL DEFAULT true,
    CONSTRAINT "PK_team_users" PRIMARY KEY ("team_id", "user_id"),
    CONSTRAINT "FK_team_users_teams_team_id" FOREIGN KEY ("team_id") REFERENCES "teams" ("id") ON DELETE CASCADE,
    CONSTRAINT "FK_team_users_users_user_id" FOREIGN KEY ("user_id") REFERENCES "users" ("id") ON DELETE CASCADE
);

-- ================================================================================================
-- INDEXES FOR ASP.NET IDENTITY TABLES
-- ================================================================================================

-- Identity Roles indexes
CREATE UNIQUE INDEX IF NOT EXISTS "RoleNameIndex" ON "AspNetRoles" ("NormalizedName") WHERE "NormalizedName" IS NOT NULL;

-- Identity Users indexes
CREATE UNIQUE INDEX IF NOT EXISTS "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName") WHERE "NormalizedUserName" IS NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS "EmailIndex" ON "AspNetUsers" ("NormalizedEmail") WHERE "NormalizedEmail" IS NOT NULL;
CREATE INDEX IF NOT EXISTS "IX_AspNetUsers_Email" ON "AspNetUsers" ("Email");

-- Identity relationship indexes
CREATE INDEX IF NOT EXISTS "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
CREATE INDEX IF NOT EXISTS "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");

-- ================================================================================================
-- INDEXES FOR CUSTOM DOMAIN TABLES
-- ================================================================================================

-- Users table indexes
CREATE UNIQUE INDEX IF NOT EXISTS "ix_users_normalized_user_name" ON "users" ("normalized_user_name") WHERE "normalized_user_name" IS NOT NULL;
CREATE UNIQUE INDEX IF NOT EXISTS "ix_users_normalized_email" ON "users" ("normalized_email") WHERE "normalized_email" IS NOT NULL;
CREATE INDEX IF NOT EXISTS "ix_users_email" ON "users" ("email");
CREATE INDEX IF NOT EXISTS "ix_users_is_active" ON "users" ("is_active");
CREATE INDEX IF NOT EXISTS "ix_users_created_at" ON "users" ("created_at");

-- Roles table indexes
CREATE UNIQUE INDEX IF NOT EXISTS "ix_roles_normalized_name" ON "roles" ("normalized_name") WHERE "normalized_name" IS NOT NULL;

-- UserRoles table indexes
CREATE INDEX IF NOT EXISTS "ix_user_roles_user_id" ON "user_roles" ("user_id");
CREATE INDEX IF NOT EXISTS "ix_user_roles_role_id" ON "user_roles" ("role_id");
CREATE INDEX IF NOT EXISTS "ix_user_roles_is_active" ON "user_roles" ("is_active");

-- Departments table indexes
CREATE UNIQUE INDEX IF NOT EXISTS "ix_departments_name" ON "departments" ("name");
CREATE INDEX IF NOT EXISTS "ix_departments_manager_id" ON "departments" ("manager_id");
CREATE INDEX IF NOT EXISTS "ix_departments_created_at" ON "departments" ("created_at");

-- Teams table indexes
CREATE INDEX IF NOT EXISTS "ix_teams_name" ON "teams" ("name");
CREATE INDEX IF NOT EXISTS "ix_teams_department_id" ON "teams" ("department_id");
CREATE INDEX IF NOT EXISTS "ix_teams_leader_id" ON "teams" ("leader_id");
CREATE INDEX IF NOT EXISTS "ix_teams_manager_id" ON "teams" ("manager_id");
CREATE INDEX IF NOT EXISTS "ix_teams_is_active" ON "teams" ("is_active");
CREATE INDEX IF NOT EXISTS "ix_teams_created_at" ON "teams" ("created_at");

-- TeamUsers table indexes
CREATE INDEX IF NOT EXISTS "ix_team_users_team_id" ON "team_users" ("team_id");
CREATE INDEX IF NOT EXISTS "ix_team_users_user_id" ON "team_users" ("user_id");
CREATE INDEX IF NOT EXISTS "ix_team_users_is_active" ON "team_users" ("is_active");
CREATE INDEX IF NOT EXISTS "ix_team_users_joined_at" ON "team_users" ("joined_at");

-- ================================================================================================
-- INITIAL DATA (OPTIONAL)
-- ================================================================================================

-- Insert default system roles (optional)
-- INSERT INTO "roles" ("id", "name", "normalized_name", "description", "is_system_role", "created_at")
-- VALUES 
--     (uuid_generate_v4(), 'Administrator', 'ADMINISTRATOR', 'System Administrator with full access', true, NOW()),
--     (uuid_generate_v4(), 'Manager', 'MANAGER', 'Department or Team Manager', true, NOW()),
--     (uuid_generate_v4(), 'User', 'USER', 'Regular system user', true, NOW());

-- ================================================================================================
-- END OF MIGRATION
-- ================================================================================================