------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserPersonInMovieRoleRatings 
-- name     : FK_UserPersonInMovieRoleRatings_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserPersonInMovieRoleRatings_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserPersonInMovieRoleRatings]'))
BEGIN
	ALTER TABLE [dbo].[UserPersonInMovieRoleRatings]  WITH CHECK ADD CONSTRAINT [FK_UserPersonInMovieRoleRatings_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
