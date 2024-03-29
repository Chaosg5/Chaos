------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconsInMovies 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconsInMovies]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconsInMovies](
		[IconId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_IconsInMovies] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInMovies_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovies]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovies] CHECK CONSTRAINT [FK_IconsInMovies_Icons]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovies]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovies] CHECK CONSTRAINT [FK_IconsInMovies_Movies]
END 

GO

------------------------------------- 
