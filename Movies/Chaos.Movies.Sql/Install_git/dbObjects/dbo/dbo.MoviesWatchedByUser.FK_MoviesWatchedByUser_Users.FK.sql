------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesWatchedByUser 
-- name     : FK_MoviesWatchedByUser_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesWatchedByUser_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesWatchedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesWatchedByUser]  WITH CHECK ADD CONSTRAINT [FK_MoviesWatchedByUser_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
