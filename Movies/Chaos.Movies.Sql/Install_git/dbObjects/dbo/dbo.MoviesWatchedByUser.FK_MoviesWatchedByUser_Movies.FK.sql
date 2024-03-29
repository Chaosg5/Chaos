------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesWatchedByUser 
-- name     : FK_MoviesWatchedByUser_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser]  WITH CHECK ADD CONSTRAINT [FK_MoviesWatchedByUser_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
