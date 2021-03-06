------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : ArtistsInGenres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[ArtistsInGenres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[ArtistsInGenres](
		[ArtistId] [int] NOT NULL,
		[GenreId] [int] NOT NULL,
	 CONSTRAINT [PK_ArtistsInGenres] PRIMARY KEY CLUSTERED 
	(
		[ArtistId] ASC,
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_ArtistsInGenres_Artists]') AND parent_object_id = OBJECT_ID(N'[music].[ArtistsInGenres]'))
BEGIN
	ALTER TABLE [music].[ArtistsInGenres] CHECK CONSTRAINT [FK_ArtistsInGenres_Artists]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_ArtistsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[ArtistsInGenres]'))
BEGIN
	ALTER TABLE [music].[ArtistsInGenres] CHECK CONSTRAINT [FK_ArtistsInGenres_Genres]
END 

GO

------------------------------------- 
