------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieReviews 
-- name     : FK_UserMovieReviews_Movies 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieReviews_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieReviews]  WITH CHECK ADD CONSTRAINT [FK_UserMovieReviews_Movies] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
