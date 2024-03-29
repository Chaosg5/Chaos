------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MoviesOwnedByUser 
-- name     : FK_MoviesOwnedByUser_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesOwnedByUser_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesOwnedByUser]'))
BEGIN
	ALTER TABLE [dbo].[MoviesOwnedByUser]  WITH CHECK ADD CONSTRAINT [FK_MoviesOwnedByUser_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
