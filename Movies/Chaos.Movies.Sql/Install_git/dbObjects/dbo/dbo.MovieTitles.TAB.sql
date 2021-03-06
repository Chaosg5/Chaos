------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieTitles](
		[MovieId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_MovieTitle] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTitle_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTitles] CHECK CONSTRAINT [FK_MovieTitle_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieTitles] CHECK CONSTRAINT [FK_MovieTitles_Languages]
END 

GO

------------------------------------- 
