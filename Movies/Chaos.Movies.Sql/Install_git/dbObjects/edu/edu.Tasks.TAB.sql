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
