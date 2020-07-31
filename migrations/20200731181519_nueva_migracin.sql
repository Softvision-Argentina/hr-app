IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200731181519_nueva_migracin')
BEGIN
    ALTER TABLE [Candidates] ADD [nueva_propiedad] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200731181519_nueva_migracin')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200731181519_nueva_migracin', N'3.1.0');
END;

GO

