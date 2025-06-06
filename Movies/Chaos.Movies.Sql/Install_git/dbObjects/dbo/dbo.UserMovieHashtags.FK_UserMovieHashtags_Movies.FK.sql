------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieHashtags 
-- name     : FK_UserMovieHashtags_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieHashtags_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieHashtags]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieHashtags]  WITH CHECK ADD CONSTRAINT [FK_UserMovieHashtags_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
