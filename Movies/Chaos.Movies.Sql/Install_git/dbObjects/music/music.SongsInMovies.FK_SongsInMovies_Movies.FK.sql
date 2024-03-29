------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : SongsInMovies 
-- name     : FK_SongsInMovies_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_SongsInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[music].[SongsInMovies]'))
BEGIN
	ALTER TABLE [music].[SongsInMovies]  WITH CHECK ADD CONSTRAINT [FK_SongsInMovies_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
