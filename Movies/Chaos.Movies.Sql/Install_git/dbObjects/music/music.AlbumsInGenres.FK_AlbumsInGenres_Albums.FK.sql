------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : AlbumsInGenres 
-- name     : FK_AlbumsInGenres_Albums 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_AlbumsInGenres_Albums]') AND parent_object_id = OBJECT_ID(N'[music].[AlbumsInGenres]'))
BEGIN
	ALTER TABLE [music].[AlbumsInGenres]  WITH CHECK ADD CONSTRAINT [FK_AlbumsInGenres_Albums] FOREIGN KEY([AlbumId])
	REFERENCES [music].[Albums] ([AlbumId])

END 

GO

------------------------------------- 
