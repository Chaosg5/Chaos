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
