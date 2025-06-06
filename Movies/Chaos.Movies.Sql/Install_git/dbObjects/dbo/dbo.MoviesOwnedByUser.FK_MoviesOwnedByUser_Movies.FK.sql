------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesOwnedByUser 
-- name     : FK_MoviesOwnedByUser_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesOwnedByUser_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesOwnedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesOwnedByUser]  WITH CHECK ADD CONSTRAINT [FK_MoviesOwnedByUser_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
