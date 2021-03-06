------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieFavorites 
-- name     : FK_UserMovieFavorites_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieFavorites_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserMovieFavorites_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
