------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : CharactersInMovies 
-- name     : FK_CharactersInMovies_Characters 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharactersInMovies_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharactersInMovies]'))
BEGIN
	ALTER TABLE [dbo].[CharactersInMovies]  WITH CHECK ADD CONSTRAINT [FK_CharactersInMovies_Characters] FOREIGN KEY([CharacterId])
	REFERENCES [dbo].[Characters] ([CharacterId])

END 

GO

------------------------------------- 
