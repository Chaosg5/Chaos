------------------------------------------------------------ 
------------------------------------------------------------ 
--- EyeDoc SQL script- CMDB.edu                       
------------------------------------------------------------ 
--- Scheme    : edu 
--- Name    : CMDB 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create Role ----------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create Schema --------------------------------- 
------------------------------------------------------------ 
-- edu.schema.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : edu 
-- Type     : schema 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'edu')
BEGIN
	EXEC sys.sp_executesql N'CREATE SCHEMA [edu]'

END 

GO

------------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ PRE Create tables ----------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create tables --------------------------------- 
------------------------------------------------------------ 
-- edu.Bounties.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : Bounties 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[Bounties]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[Bounties](
		[BountyId] [int] IDENTITY(1,1) NOT NULL,
		[BountyTypeId] [int] NOT NULL,
		[BountyValue] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_Bounties] PRIMARY KEY CLUSTERED 
	(
		[BountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Bounties_BountyTypes]') AND parent_object_id = OBJECT_ID(N'[edu].[Bounties]'))
BEGIN
	ALTER TABLE [edu].[Bounties] CHECK CONSTRAINT [FK_Bounties_BountyTypes]
END 

GO

------------------------------------- 
-- edu.BountyTypes.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : BountyTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[BountyTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[BountyTypes](
		[BountyTypeId] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_BountyTypes] PRIMARY KEY CLUSTERED 
	(
		[BountyTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- edu.Subjects.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : Subjects 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[Subjects]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[Subjects](
		[SubjectId] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED 
	(
		[SubjectId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
-- edu.TaskBounties.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : TaskBounties 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[TaskBounties]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[TaskBounties](
		[TaskBountyId] [int] IDENTITY(1,1) NOT NULL,
		[TaskId] [int] NOT NULL,
		[BountyId] [int] NOT NULL,
		[DurationInMinutes] [int] NOT NULL,
	 CONSTRAINT [PK_TaskBounties] PRIMARY KEY CLUSTERED 
	(
		[TaskBountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_TaskBounties_Bounties]') AND parent_object_id = OBJECT_ID(N'[edu].[TaskBounties]'))
BEGIN
	ALTER TABLE [edu].[TaskBounties] CHECK CONSTRAINT [FK_TaskBounties_Bounties]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_TaskBounties_Tasks]') AND parent_object_id = OBJECT_ID(N'[edu].[TaskBounties]'))
BEGIN
	ALTER TABLE [edu].[TaskBounties] CHECK CONSTRAINT [FK_TaskBounties_Tasks]
END 

GO

------------------------------------- 
-- edu.Tasks.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : Tasks 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[Tasks]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[Tasks](
		[TaskId] [int] IDENTITY(1,1) NOT NULL,
		[SubjectId] [int] NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
	 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
	(
		[TaskId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Tasks_Subjects]') AND parent_object_id = OBJECT_ID(N'[edu].[Tasks]'))
BEGIN
	ALTER TABLE [edu].[Tasks] CHECK CONSTRAINT [FK_Tasks_Subjects]
END 

GO

------------------------------------- 
-- edu.UserTaskBounties.TAB.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : UserTaskBounties 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[UserTaskBounties]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[UserTaskBounties](
		[UserTaskBountyId] [int] IDENTITY(1,1) NOT NULL,
		[UserId] [int] NOT NULL,
		[TaskBountyId] [int] NOT NULL,
		[AssignedBy] [int] NOT NULL,
		[AssignedAt] [datetime2](0) NOT NULL,
		[StartedAt] [datetime2](0) NULL,
		[CompletedAt] [datetime2](0) NULL,
		[AwardedAt] [datetime2](0) NULL,
		[AwardedBy] [int] NULL,
		[Result] [nvarchar](max) NULL,
	 CONSTRAINT [PK_UserTaskBounties] PRIMARY KEY CLUSTERED 
	(
		[UserTaskBountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_TaskBounties]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_TaskBounties]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_Users]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users1]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_Users1]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users2]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties] CHECK CONSTRAINT [FK_UserTaskBounties_Users2]
END 

GO

------------------------------------- 
------------------------------------------------------------ 
------------ Update tables --------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create TYPE ----------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Set dafault data ------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Set config data ------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create CONSTRAINT ----------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Update CONSTRAINT ----------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create CHECK CONSTRAINT ----------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Update CHECK CONSTRAINT ----------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create REFERENCES ----------------------------- 
------------------------------------------------------------ 
-- edu.Bounties.FK_Bounties_BountyTypes.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : Bounties 
-- name     : FK_Bounties_BountyTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Bounties_BountyTypes]') AND parent_object_id = OBJECT_ID(N'[edu].[Bounties]'))
BEGIN
	ALTER TABLE [edu].[Bounties]  WITH CHECK ADD CONSTRAINT [FK_Bounties_BountyTypes] FOREIGN KEY([BountyTypeId])
	REFERENCES [edu].[BountyTypes] ([BountyTypeId])

END 

GO

------------------------------------- 
-- edu.TaskBounties.FK_TaskBounties_Bounties.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : TaskBounties 
-- name     : FK_TaskBounties_Bounties 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_TaskBounties_Bounties]') AND parent_object_id = OBJECT_ID(N'[edu].[TaskBounties]'))
BEGIN
	ALTER TABLE [edu].[TaskBounties]  WITH CHECK ADD CONSTRAINT [FK_TaskBounties_Bounties] FOREIGN KEY([BountyId])
	REFERENCES [edu].[Bounties] ([BountyId])

END 

GO

------------------------------------- 
-- edu.TaskBounties.FK_TaskBounties_Tasks.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : TaskBounties 
-- name     : FK_TaskBounties_Tasks 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_TaskBounties_Tasks]') AND parent_object_id = OBJECT_ID(N'[edu].[TaskBounties]'))
BEGIN
	ALTER TABLE [edu].[TaskBounties]  WITH CHECK ADD CONSTRAINT [FK_TaskBounties_Tasks] FOREIGN KEY([TaskId])
	REFERENCES [edu].[Tasks] ([TaskId])

END 

GO

------------------------------------- 
-- edu.Tasks.FK_Tasks_Subjects.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : Tasks 
-- name     : FK_Tasks_Subjects 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Tasks_Subjects]') AND parent_object_id = OBJECT_ID(N'[edu].[Tasks]'))
BEGIN
	ALTER TABLE [edu].[Tasks]  WITH CHECK ADD CONSTRAINT [FK_Tasks_Subjects] FOREIGN KEY([SubjectId])
	REFERENCES [edu].[Subjects] ([SubjectId])

END 

GO

------------------------------------- 
-- edu.UserTaskBounties.FK_UserTaskBounties_TaskBounties.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : UserTaskBounties 
-- name     : FK_UserTaskBounties_TaskBounties 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_TaskBounties]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD CONSTRAINT [FK_UserTaskBounties_TaskBounties] FOREIGN KEY([TaskBountyId])
	REFERENCES [edu].[TaskBounties] ([TaskBountyId])

END 

GO

------------------------------------- 
-- edu.UserTaskBounties.FK_UserTaskBounties_Users.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : UserTaskBounties 
-- name     : FK_UserTaskBounties_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD CONSTRAINT [FK_UserTaskBounties_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- edu.UserTaskBounties.FK_UserTaskBounties_Users1.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : UserTaskBounties 
-- name     : FK_UserTaskBounties_Users1 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users1]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD CONSTRAINT [FK_UserTaskBounties_Users1] FOREIGN KEY([AssignedBy])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
-- edu.UserTaskBounties.FK_UserTaskBounties_Users2.FK.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : UserTaskBounties 
-- name     : FK_UserTaskBounties_Users2 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users2]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD CONSTRAINT [FK_UserTaskBounties_Users2] FOREIGN KEY([AwardedBy])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
------------------------------------------------------------ 
------------ Update REFERENCES ----------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Update INDEX ---------------------------------- 
------------------------------------------------------------ 
------------ Update Unique INDEX --------------------------- 
------------ Update Non-Unique INDEX ----------------------- 
-- edu.TaskBounties.UX_TaskBounties.IX.sql 
------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : TaskBounties 
-- name     : UX_TaskBounties 
-- Type     : nonuniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[edu].[TaskBounties]') AND name = N'UX_TaskBounties')
BEGIN
	CREATE NONCLUSTERED INDEX [UX_TaskBounties] ON [edu].[TaskBounties]
	(
		[TaskId] ASC,
		[BountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create userdefind functions ------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create Views----------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create CONSTRAINT UNIQUE NONCLUSTERED --------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Update CONSTRAINT UNIQUE NONCLUSTERED --------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Create Stored procedures----------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Remove Objects -------------------------------- 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------------------------------------------------------ 
------------ Rights ---------------------------------------- 
------------------------------------------------------------ 
