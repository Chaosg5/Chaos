------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : VoiceOverActors 
-- name     : FK_VoiceOverActors_CharactersInMovies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VoiceOverActors_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[VoiceOverActors]'))
BEGIN
	ALTER TABLE [dbo].[VoiceOverActors]  WITH CHECK ADD CONSTRAINT [FK_VoiceOverActors_CharactersInMovies] FOREIGN KEY([CharacterInMovieId])
	REFERENCES [dbo].[CharactersInMovies] ([CharacterInMovieId])

END 

GO

------------------------------------- 
