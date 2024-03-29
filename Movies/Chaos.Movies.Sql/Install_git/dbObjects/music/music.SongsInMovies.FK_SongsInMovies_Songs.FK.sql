------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : SongsInMovies 
-- name     : FK_SongsInMovies_Songs 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInMovies_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInMovies]'))
BEGIN
	ALTER TABLE [music].[SongsInMovies]  WITH CHECK ADD CONSTRAINT [FK_SongsInMovies_Songs] FOREIGN KEY([SongId])
	REFERENCES [music].[Songs] ([SongId])

END 

GO

------------------------------------- 
