------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserPeopleFavorites 
-- name     : FK_UserPeopleFavorites_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPeopleFavorites_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPeopleFavorites]'))
BEGIN
	ALTER TABLE [dbo].[UserPeopleFavorites]  WITH CHECK ADD CONSTRAINT [FK_UserPeopleFavorites_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
