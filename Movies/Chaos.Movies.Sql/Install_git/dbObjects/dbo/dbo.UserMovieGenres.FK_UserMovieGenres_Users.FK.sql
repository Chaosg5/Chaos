------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieGenres 
-- name     : FK_UserMovieGenres_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieGenres_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieGenres]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieGenres]  WITH CHECK ADD CONSTRAINT [FK_UserMovieGenres_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
