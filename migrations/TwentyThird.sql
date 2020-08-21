IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200820185609_TwentyThird')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PreOfferStages]') AND [c].[name] = N'PreocupationalDone');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [PreOfferStages] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [PreOfferStages] ALTER COLUMN [PreocupationalDone] bit NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200820185609_TwentyThird')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PreOfferStages]') AND [c].[name] = N'BackgroundCheckDone');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [PreOfferStages] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [PreOfferStages] ALTER COLUMN [BackgroundCheckDone] bit NULL;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200820185609_TwentyThird')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200820185609_TwentyThird', N'3.1.0');
END;

GO

