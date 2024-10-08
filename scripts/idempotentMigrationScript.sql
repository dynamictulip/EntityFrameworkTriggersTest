﻿IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240912105911_InitialCreate'
)
BEGIN
    CREATE TABLE [Blogs] (
        [BlogId] int NOT NULL IDENTITY,
        [Url] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Blogs] PRIMARY KEY ([BlogId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240912105911_InitialCreate'
)
BEGIN
    CREATE TABLE [Posts] (
        [PostId] int NOT NULL IDENTITY,
        [Title] nvarchar(max) NOT NULL,
        [Content] nvarchar(max) NOT NULL,
        [BlogId] int NOT NULL,
        CONSTRAINT [PK_Posts] PRIMARY KEY ([PostId]),
        CONSTRAINT [FK_Posts_Blogs_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Blogs] ([BlogId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240912105911_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Posts_BlogId] ON [Posts] ([BlogId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240912105911_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240912105911_InitialCreate', N'8.0.8');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240912234001_AuditTable'
)
BEGIN
    CREATE TABLE [AuditEvents] (
        [Id] uniqueidentifier NOT NULL,
        [TableName] nvarchar(max) NOT NULL,
        [PrimaryKeyColumnName] nvarchar(max) NOT NULL,
        [PrimaryKeyType] nvarchar(max) NOT NULL,
        [PrimaryKeyValueAsJson] nvarchar(max) NOT NULL,
        [EntityOperationType] int NOT NULL,
        [AlteredColumnName] nvarchar(max) NOT NULL,
        [AlteredColumnType] nvarchar(max) NOT NULL,
        [AuditMessage] nvarchar(max) NULL,
        CONSTRAINT [PK_AuditEvents] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240912234001_AuditTable'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240912234001_AuditTable', N'8.0.8');
END;
GO

COMMIT;
GO

