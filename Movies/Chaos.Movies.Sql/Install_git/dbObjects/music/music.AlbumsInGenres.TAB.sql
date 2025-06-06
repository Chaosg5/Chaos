------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : AlbumsInGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[AlbumsInGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[AlbumsInGenres](
		[AlbumId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
	 CONSTRAINT [PK_AlbumsInGenres] PRIMARY KEY CLUSTERED 
	(
		[AlbumId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_AlbumsInGenres_Albums]') AND parent_object_id = OBJECT_ID(N'[music].[AlbumsInGenres]'))
BEGIN
	ALTER TABLE [music].[AlbumsInGenres] CHECK CONSTRAINT [FK_AlbumsInGenres_Albums]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_AlbumsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[AlbumsInGenres]'))
BEGIN
	ALTER TABLE [music].[AlbumsInGenres] CHECK CONSTRAINT [FK_AlbumsInGenres_Genres]
END 

GO

------------------------------------- 
