------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInMovies 
-- name     : FK_IconsInMovies_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInMovies_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInMovies]'))
BEGIN
	ALTER TABLE [dbo].[IconsInMovies]  WITH CHECK ADD CONSTRAINT [FK_IconsInMovies_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
