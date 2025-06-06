------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MoviesInGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoviesInGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MoviesInGenres](
		[MovieId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
	 CONSTRAINT [PK_MoviesInGenres] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInGenres]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInGenres] CHECK CONSTRAINT [FK_MoviesInGenres_Genres]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInGenres_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInGenres]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInGenres] CHECK CONSTRAINT [FK_MoviesInGenres_Movies]
END 

GO

------------------------------------- 
