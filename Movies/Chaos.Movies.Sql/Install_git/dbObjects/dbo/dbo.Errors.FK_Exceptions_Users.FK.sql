------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Errors 
-- name     : FK_Exceptions_Users 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Exceptions_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Errors]'))
BEGIN
	ALTER TABLE [dbo].[Errors]  WITH CHECK ADD CONSTRAINT [FK_Exceptions_Users] FOREIGN KEY([UserId])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
