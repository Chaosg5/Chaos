------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieTypeTitles](
		[MovieTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_MovieTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[MovieTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTypeTitles] CHECK CONSTRAINT [FK_MovieTypeTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTypeTitles_MovieTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTypeTitles] CHECK CONSTRAINT [FK_MovieTypeTitles_MovieTypes]
END 

GO

------------------------------------- 
