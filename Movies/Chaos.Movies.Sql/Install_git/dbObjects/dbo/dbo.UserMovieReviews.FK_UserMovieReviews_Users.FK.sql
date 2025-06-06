------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieReviews 
-- name     : FK_UserMovieReviews_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieReviews_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieReviews]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieReviews]  WITH CHECK ADD CONSTRAINT [FK_UserMovieReviews_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
