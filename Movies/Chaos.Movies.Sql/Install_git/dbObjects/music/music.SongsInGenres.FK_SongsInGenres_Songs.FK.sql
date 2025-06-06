------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : SongsInGenres 
-- name     : FK_SongsInGenres_Songs 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInGenres_Songs]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInGenres]'))
BEGIN
	ALTER TABLE [music].[SongsInGenres]  WITH CHECK ADD CONSTRAINT [FK_SongsInGenres_Songs] FOREIGN KEY([SongId])
	REFERENCES [music].[Songs] ([SongId])

END 

GO

------------------------------------- 
