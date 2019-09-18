﻿/*
Deployment script for AMS_App_PROD

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "AMS_App_PROD"
:setvar DefaultFilePrefix "AMS_App_PROD"
:setvar DefaultDataPath "E:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\Data\"
:setvar DefaultLogPath "L:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\Data_Log\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Disabling all DDL triggers...'
GO
DISABLE TRIGGER ALL ON DATABASE
GO
PRINT N'Creating [tasks]...';


GO
CREATE SCHEMA [tasks]
    AUTHORIZATION [dbo];


GO
PRINT N'Creating [tasks].[Assignees]...';


GO
CREATE TABLE [tasks].[Assignees] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [ActiveDirectoryName] NVARCHAR (MAX)   NULL,
    [CreatedBy]           NVARCHAR (MAX)   NULL,
    [CreatedDate]         DATETIME2 (7)    NOT NULL,
    [LastModifiedBy]      NVARCHAR (MAX)   NULL,
    [LastModifiedDate]    DATETIME2 (7)    NOT NULL,
    [Version]             BIGINT           NOT NULL,
    CONSTRAINT [PK_Assignees] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [tasks].[TaskStatus]...';


GO
CREATE TABLE [tasks].[TaskStatus] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]        NVARCHAR (MAX)   NULL,
    [CreatedDate]      DATETIME2 (7)    NOT NULL,
    [LastModifiedBy]   NVARCHAR (MAX)   NULL,
    [LastModifiedDate] DATETIME2 (7)    NOT NULL,
    [TaskId]           UNIQUEIDENTIFIER NOT NULL,
    [TaskStateId]      UNIQUEIDENTIFIER NOT NULL,
    [Version]          BIGINT           NOT NULL,
    CONSTRAINT [PK_TaskStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [tasks].[TaskStatus].[IX_TaskStatus_TaskId]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TaskStatus_TaskId]
    ON [tasks].[TaskStatus]([TaskId] ASC);


GO
PRINT N'Creating [tasks].[TaskStatus].[IX_TaskStatus_TaskStateId]...';


GO
CREATE NONCLUSTERED INDEX [IX_TaskStatus_TaskStateId]
    ON [tasks].[TaskStatus]([TaskStateId] ASC);


GO
PRINT N'Creating [tasks].[TaskAssignee]...';


GO
CREATE TABLE [tasks].[TaskAssignee] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [ActionedOnDate]   DATETIME2 (7)    NULL,
    [AssigneeId]       UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]        NVARCHAR (MAX)   NULL,
    [CreatedDate]      DATETIME2 (7)    NOT NULL,
    [LastModifiedBy]   NVARCHAR (MAX)   NULL,
    [LastModifiedDate] DATETIME2 (7)    NOT NULL,
    [TaskId]           UNIQUEIDENTIFIER NOT NULL,
    [Version]          BIGINT           NOT NULL,
    CONSTRAINT [PK_TaskAssignee] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [tasks].[TaskAssignee].[IX_TaskAssignee_AssigneeId]...';


GO
CREATE NONCLUSTERED INDEX [IX_TaskAssignee_AssigneeId]
    ON [tasks].[TaskAssignee]([AssigneeId] ASC);


GO
PRINT N'Creating [tasks].[TaskAssignee].[IX_TaskAssignee_TaskId]...';


GO
CREATE NONCLUSTERED INDEX [IX_TaskAssignee_TaskId]
    ON [tasks].[TaskAssignee]([TaskId] ASC);


GO
PRINT N'Creating [tasks].[TaskStates]...';


GO
CREATE TABLE [tasks].[TaskStates] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]        NVARCHAR (MAX)   NULL,
    [CreatedDate]      DATETIME2 (7)    NOT NULL,
    [Description]      NVARCHAR (MAX)   NULL,
    [LastModifiedBy]   NVARCHAR (MAX)   NULL,
    [LastModifiedDate] DATETIME2 (7)    NOT NULL,
    [Name]             NVARCHAR (MAX)   NULL,
    [Step]             INT              NOT NULL,
    [TaskTypeId]       UNIQUEIDENTIFIER NULL,
    [Version]          BIGINT           NOT NULL,
    CONSTRAINT [PK_TaskStates] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [tasks].[TaskStates].[IX_TaskStates_TaskTypeId]...';


GO
CREATE NONCLUSTERED INDEX [IX_TaskStates_TaskTypeId]
    ON [tasks].[TaskStates]([TaskTypeId] ASC);


GO
PRINT N'Creating [tasks].[Tasks]...';


GO
CREATE TABLE [tasks].[Tasks] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
    [ClientSystemId]       UNIQUEIDENTIFIER NOT NULL,
    [ContextId]            UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]            NVARCHAR (MAX)   NULL,
    [CreatedDate]          DATETIME2 (7)    NOT NULL,
    [Description]          NVARCHAR (MAX)   NULL,
    [EndDate]              DATETIME2 (7)    NOT NULL,
    [LastModifiedBy]       NVARCHAR (MAX)   NULL,
    [LastModifiedDate]     DATETIME2 (7)    NOT NULL,
    [Metadata]             NVARCHAR (MAX)   NULL,
    [MinAssigneesApproval] INT              NOT NULL,
    [Name]                 NVARCHAR (MAX)   NULL,
    [Order]                INT              NOT NULL,
    [ParentTaskId]         UNIQUEIDENTIFIER NULL,
    [RecordStatus]         INT              NOT NULL,
    [StartDate]            DATETIME2 (7)    NOT NULL,
    [TaskTypeId]           UNIQUEIDENTIFIER NOT NULL,
    [Version]              BIGINT           NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [tasks].[Tasks].[IX_Tasks_ClientSystemId]...';


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_ClientSystemId]
    ON [tasks].[Tasks]([ClientSystemId] ASC);


GO
PRINT N'Creating [tasks].[Tasks].[IX_Tasks_ParentTaskId]...';


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_ParentTaskId]
    ON [tasks].[Tasks]([ParentTaskId] ASC);


GO
PRINT N'Creating [tasks].[Tasks].[IX_Tasks_TaskTypeId]...';


GO
CREATE NONCLUSTERED INDEX [IX_Tasks_TaskTypeId]
    ON [tasks].[Tasks]([TaskTypeId] ASC);


GO
PRINT N'Creating [tasks].[TaskTypes]...';


GO
CREATE TABLE [tasks].[TaskTypes] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
    [ClientSystemId]       UNIQUEIDENTIFIER NULL,
    [CreatedBy]            NVARCHAR (MAX)   NULL,
    [CreatedDate]          DATETIME2 (7)    NOT NULL,
    [Description]          NVARCHAR (MAX)   NULL,
    [LastModifiedBy]       NVARCHAR (MAX)   NULL,
    [LastModifiedDate]     DATETIME2 (7)    NOT NULL,
    [MinAssigneesApproval] INT              NOT NULL,
    [Name]                 NVARCHAR (MAX)   NULL,
    [Version]              BIGINT           NOT NULL,
    CONSTRAINT [PK_TaskTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [tasks].[TaskTypes].[IX_TaskTypes_ClientSystemId]...';


GO
CREATE NONCLUSTERED INDEX [IX_TaskTypes_ClientSystemId]
    ON [tasks].[TaskTypes]([ClientSystemId] ASC);


GO
PRINT N'Creating [tasks].[ClientSystems]...';


GO
CREATE TABLE [tasks].[ClientSystems] (
    [Id]                        UNIQUEIDENTIFIER NOT NULL,
    [CreatedBy]                 NVARCHAR (MAX)   NULL,
    [CreatedDate]               DATETIME2 (7)    NOT NULL,
    [Description]               NVARCHAR (MAX)   NULL,
    [LastModifiedBy]            NVARCHAR (MAX)   NULL,
    [LastModifiedDate]          DATETIME2 (7)    NOT NULL,
    [Name]                      NVARCHAR (MAX)   NULL,
    [Version]                   BIGINT           NOT NULL,
    [ActiveDirectoryAccessList] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_ClientSystems] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating unnamed constraint on [tasks].[Assignees]...';


GO
ALTER TABLE [tasks].[Assignees]
    ADD DEFAULT (user_name()) FOR [CreatedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[Assignees]...';


GO
ALTER TABLE [tasks].[Assignees]
    ADD DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[Assignees]...';


GO
ALTER TABLE [tasks].[Assignees]
    ADD DEFAULT (user_name()) FOR [LastModifiedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[Assignees]...';


GO
ALTER TABLE [tasks].[Assignees]
    ADD DEFAULT (getdate()) FOR [LastModifiedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskStatus]...';


GO
ALTER TABLE [tasks].[TaskStatus]
    ADD DEFAULT (user_name()) FOR [CreatedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskStatus]...';


GO
ALTER TABLE [tasks].[TaskStatus]
    ADD DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskStatus]...';


GO
ALTER TABLE [tasks].[TaskStatus]
    ADD DEFAULT (user_name()) FOR [LastModifiedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskStatus]...';


GO
ALTER TABLE [tasks].[TaskStatus]
    ADD DEFAULT (getdate()) FOR [LastModifiedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskAssignee]...';


GO
ALTER TABLE [tasks].[TaskAssignee]
    ADD DEFAULT (user_name()) FOR [CreatedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskAssignee]...';


GO
ALTER TABLE [tasks].[TaskAssignee]
    ADD DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskAssignee]...';


GO
ALTER TABLE [tasks].[TaskAssignee]
    ADD DEFAULT (user_name()) FOR [LastModifiedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskAssignee]...';


GO
ALTER TABLE [tasks].[TaskAssignee]
    ADD DEFAULT (getdate()) FOR [LastModifiedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskStates]...';


GO
ALTER TABLE [tasks].[TaskStates]
    ADD DEFAULT (user_name()) FOR [CreatedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskStates]...';


GO
ALTER TABLE [tasks].[TaskStates]
    ADD DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskStates]...';


GO
ALTER TABLE [tasks].[TaskStates]
    ADD DEFAULT (user_name()) FOR [LastModifiedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskStates]...';


GO
ALTER TABLE [tasks].[TaskStates]
    ADD DEFAULT (getdate()) FOR [LastModifiedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[Tasks]...';


GO
ALTER TABLE [tasks].[Tasks]
    ADD DEFAULT (user_name()) FOR [CreatedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[Tasks]...';


GO
ALTER TABLE [tasks].[Tasks]
    ADD DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[Tasks]...';


GO
ALTER TABLE [tasks].[Tasks]
    ADD DEFAULT (user_name()) FOR [LastModifiedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[Tasks]...';


GO
ALTER TABLE [tasks].[Tasks]
    ADD DEFAULT (getdate()) FOR [LastModifiedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[Tasks]...';


GO
ALTER TABLE [tasks].[Tasks]
    ADD DEFAULT ((-1)) FOR [MinAssigneesApproval];


GO
PRINT N'Creating unnamed constraint on [tasks].[Tasks]...';


GO
ALTER TABLE [tasks].[Tasks]
    ADD DEFAULT ((0)) FOR [Order];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskTypes]...';


GO
ALTER TABLE [tasks].[TaskTypes]
    ADD DEFAULT (user_name()) FOR [CreatedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskTypes]...';


GO
ALTER TABLE [tasks].[TaskTypes]
    ADD DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskTypes]...';


GO
ALTER TABLE [tasks].[TaskTypes]
    ADD DEFAULT (user_name()) FOR [LastModifiedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[TaskTypes]...';


GO
ALTER TABLE [tasks].[TaskTypes]
    ADD DEFAULT (getdate()) FOR [LastModifiedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[ClientSystems]...';


GO
ALTER TABLE [tasks].[ClientSystems]
    ADD DEFAULT (user_name()) FOR [CreatedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[ClientSystems]...';


GO
ALTER TABLE [tasks].[ClientSystems]
    ADD DEFAULT (getdate()) FOR [CreatedDate];


GO
PRINT N'Creating unnamed constraint on [tasks].[ClientSystems]...';


GO
ALTER TABLE [tasks].[ClientSystems]
    ADD DEFAULT (user_name()) FOR [LastModifiedBy];


GO
PRINT N'Creating unnamed constraint on [tasks].[ClientSystems]...';


GO
ALTER TABLE [tasks].[ClientSystems]
    ADD DEFAULT (getdate()) FOR [LastModifiedDate];


GO
PRINT N'Creating [tasks].[FK_TaskStatus_Tasks_TaskId]...';


GO
ALTER TABLE [tasks].[TaskStatus] WITH NOCHECK
    ADD CONSTRAINT [FK_TaskStatus_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [tasks].[Tasks] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [tasks].[FK_TaskStatus_TaskStates_TaskStateId]...';


GO
ALTER TABLE [tasks].[TaskStatus] WITH NOCHECK
    ADD CONSTRAINT [FK_TaskStatus_TaskStates_TaskStateId] FOREIGN KEY ([TaskStateId]) REFERENCES [tasks].[TaskStates] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [tasks].[FK_TaskAssignee_Assignees_AssigneeId]...';


GO
ALTER TABLE [tasks].[TaskAssignee] WITH NOCHECK
    ADD CONSTRAINT [FK_TaskAssignee_Assignees_AssigneeId] FOREIGN KEY ([AssigneeId]) REFERENCES [tasks].[Assignees] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [tasks].[FK_TaskAssignee_Tasks_TaskId]...';


GO
ALTER TABLE [tasks].[TaskAssignee] WITH NOCHECK
    ADD CONSTRAINT [FK_TaskAssignee_Tasks_TaskId] FOREIGN KEY ([TaskId]) REFERENCES [tasks].[Tasks] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [tasks].[FK_TaskStates_TaskTypes_TaskTypeId]...';


GO
ALTER TABLE [tasks].[TaskStates] WITH NOCHECK
    ADD CONSTRAINT [FK_TaskStates_TaskTypes_TaskTypeId] FOREIGN KEY ([TaskTypeId]) REFERENCES [tasks].[TaskTypes] ([Id]);


GO
PRINT N'Creating [tasks].[FK_Tasks_ClientSystems_ClientSystemId]...';


GO
ALTER TABLE [tasks].[Tasks] WITH NOCHECK
    ADD CONSTRAINT [FK_Tasks_ClientSystems_ClientSystemId] FOREIGN KEY ([ClientSystemId]) REFERENCES [tasks].[ClientSystems] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [tasks].[FK_Tasks_Tasks_ParentTaskId]...';


GO
ALTER TABLE [tasks].[Tasks] WITH NOCHECK
    ADD CONSTRAINT [FK_Tasks_Tasks_ParentTaskId] FOREIGN KEY ([ParentTaskId]) REFERENCES [tasks].[Tasks] ([Id]);


GO
PRINT N'Creating [tasks].[FK_Tasks_TaskTypes_TaskTypeId]...';


GO
ALTER TABLE [tasks].[Tasks] WITH NOCHECK
    ADD CONSTRAINT [FK_Tasks_TaskTypes_TaskTypeId] FOREIGN KEY ([TaskTypeId]) REFERENCES [tasks].[TaskTypes] ([Id]) ON DELETE CASCADE;


GO
PRINT N'Creating [tasks].[FK_TaskTypes_ClientSystems_ClientSystemId]...';


GO
ALTER TABLE [tasks].[TaskTypes] WITH NOCHECK
    ADD CONSTRAINT [FK_TaskTypes_ClientSystems_ClientSystemId] FOREIGN KEY ([ClientSystemId]) REFERENCES [tasks].[ClientSystems] ([Id]);


GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [tasks].[TaskStatus] WITH CHECK CHECK CONSTRAINT [FK_TaskStatus_Tasks_TaskId];

ALTER TABLE [tasks].[TaskStatus] WITH CHECK CHECK CONSTRAINT [FK_TaskStatus_TaskStates_TaskStateId];

ALTER TABLE [tasks].[TaskAssignee] WITH CHECK CHECK CONSTRAINT [FK_TaskAssignee_Assignees_AssigneeId];

ALTER TABLE [tasks].[TaskAssignee] WITH CHECK CHECK CONSTRAINT [FK_TaskAssignee_Tasks_TaskId];

ALTER TABLE [tasks].[TaskStates] WITH CHECK CHECK CONSTRAINT [FK_TaskStates_TaskTypes_TaskTypeId];

ALTER TABLE [tasks].[Tasks] WITH CHECK CHECK CONSTRAINT [FK_Tasks_ClientSystems_ClientSystemId];

ALTER TABLE [tasks].[Tasks] WITH CHECK CHECK CONSTRAINT [FK_Tasks_Tasks_ParentTaskId];

ALTER TABLE [tasks].[Tasks] WITH CHECK CHECK CONSTRAINT [FK_Tasks_TaskTypes_TaskTypeId];

ALTER TABLE [tasks].[TaskTypes] WITH CHECK CHECK CONSTRAINT [FK_TaskTypes_ClientSystems_ClientSystemId];


GO
PRINT N'Reenabling DDL triggers...'
GO
ENABLE TRIGGER [MCAFEE_SENSOR_DB_DDL_TRIGGER] ON DATABASE
GO
PRINT N'Update complete.';


GO
