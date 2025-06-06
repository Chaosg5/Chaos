------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : ArtistsInGenres 
-- name     : FK_ArtistsInGenres_Genres 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_ArtistsInGenres_Genres]') AND parent_object_id = OBJECT_ID(N'[music].[ArtistsInGenres]'))
BEGIN
	ALTER TABLE [music].[ArtistsInGenres]  WITH CHECK ADD CONSTRAINT [FK_ArtistsInGenres_Genres] FOREIGN KEY([GenreId])
	REFERENCES [music].[Genres] ([GenreId])

END 

GO

------------------------------------- 
