------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : WatchTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WatchTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[WatchTypeTitles](
		[WatchTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_WatchTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[WatchTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WatchTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[WatchTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[WatchTypeTitles] CHECK CONSTRAINT [FK_WatchTypeTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WatchTypeTitles_WatchTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[WatchTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[WatchTypeTitles] CHECK CONSTRAINT [FK_WatchTypeTitles_WatchTypes]
END 

GO

------------------------------------- 
