------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserCharacterInMovieRatings 
-- name     : FK_UserCharacterInMovieRatings_CharactersInMovies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserCharacterInMovieRatings_CharactersInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserCharacterInMovieRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserCharacterInMovieRatings]  WITH CHECK ADD CONSTRAINT [FK_UserCharacterInMovieRatings_CharactersInMovies] FOREIGN KEY([CharacterInMovieId])
	REFERENCES [dbo].[CharactersInMovies] ([CharacterInMovieId])

END 

GO

------------------------------------- 
