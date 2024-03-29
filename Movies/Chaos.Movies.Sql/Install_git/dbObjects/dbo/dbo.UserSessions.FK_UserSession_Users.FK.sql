------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : UserSessions 
-- name     : FK_UserSession_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserSession_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserSessions]'))
BEGIN
	ALTER TABLE [dbo].[UserSessions]  WITH CHECK ADD CONSTRAINT [FK_UserSession_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
