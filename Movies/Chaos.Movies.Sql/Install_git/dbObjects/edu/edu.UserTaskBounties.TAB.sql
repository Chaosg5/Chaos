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
