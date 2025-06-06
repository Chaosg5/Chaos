------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : SongsInGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[SongsInGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[SongsInGenres](
		[SongId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
	 CONSTRAINT [PK_SongsInGenres] PRIMARY KEY CLUSTERED 
	(
		[SongId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInGenres]'))
BEGIN
	ALTER TABLE [music].[SongsInGenres] CHECK CONSTRAINT [FK_SongsInGenres_Genres]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInGenres_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInGenres]'))
BEGIN
	ALTER TABLE [music].[SongsInGenres] CHECK CONSTRAINT [FK_SongsInGenres_Songs]
END 

GO

------------------------------------- 
