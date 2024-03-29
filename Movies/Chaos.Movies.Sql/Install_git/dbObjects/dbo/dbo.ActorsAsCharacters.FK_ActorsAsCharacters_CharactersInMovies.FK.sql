------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : ActorsAsCharacters 
-- name     : FK_ActorsAsCharacters_CharactersInMovies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ActorsAsCharacters_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[ActorsAsCharacters]'))
BEGIN
	ALTER TABLE [dbo].[ActorsAsCharacters]  WITH CHECK ADD CONSTRAINT [FK_ActorsAsCharacters_CharactersInMovies] FOREIGN KEY([CharacterInMovieId])
	REFERENCES [dbo].[CharactersInMovies] ([CharacterInMovieId])

END 

GO

------------------------------------- 
