------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserMovieWishlist 
-- name     : FK_UserMovieWishlist_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserMovieWishlist_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserMovieWishlist]'))
BEGIN
	ALTER TABLE [dbo].[UserMovieWishlist]  WITH CHECK ADD CONSTRAINT [FK_UserMovieWishlist_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
