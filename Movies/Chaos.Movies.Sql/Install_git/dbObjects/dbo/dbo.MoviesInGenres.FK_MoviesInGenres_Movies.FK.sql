------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesInGenres 
-- name     : FK_MoviesInGenres_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInGenres_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInGenres]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInGenres]  WITH CHECK ADD CONSTRAINT [FK_MoviesInGenres_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
