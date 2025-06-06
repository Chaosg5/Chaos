------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : SongsInMovies 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[SongsInMovies]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[SongsInMovies](
		[SongId] [int] NOT NULL,
		[MovieId] [int] NOT NULL,
	 CONSTRAINT [PK_SongsInMovies] PRIMARY KEY CLUSTERED 
	(
		[SongId] ASC,
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInMovies]'))
BEGIN
	ALTER TABLE [music].[SongsInMovies] CHECK CONSTRAINT [FK_SongsInMovies_Movies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInMovies_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInMovies]'))
BEGIN
	ALTER TABLE [music].[SongsInMovies] CHECK CONSTRAINT [FK_SongsInMovies_Songs]
END 

GO

------------------------------------- 
