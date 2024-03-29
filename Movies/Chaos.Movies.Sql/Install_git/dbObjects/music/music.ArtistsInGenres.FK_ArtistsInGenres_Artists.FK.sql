------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : ArtistsInGenres 
-- name     : FK_ArtistsInGenres_Artists 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_ArtistsInGenres_Artists]') AND parent_object_id = OBJECT_ID(N'[music].[ArtistsInGenres]'))
BEGIN
	ALTER TABLE [music].[ArtistsInGenres]  WITH CHECK ADD CONSTRAINT [FK_ArtistsInGenres_Artists] FOREIGN KEY([ArtistId])
	REFERENCES [music].[Artists] ([ArtistId])

END 

GO

------------------------------------- 
