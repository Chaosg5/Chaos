------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : UserTaskBounties 
-- name     : FK_UserTaskBounties_Users1 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_UserTaskBounties_Users1]') AND parent_object_id = OBJECT_ID(N'[edu].[UserTaskBounties]'))
BEGIN
	ALTER TABLE [edu].[UserTaskBounties]  WITH CHECK ADD CONSTRAINT [FK_UserTaskBounties_Users1] FOREIGN KEY([AssignedBy])
	REFERENCES [dbo].[Users] ([UserId])

END 

GO

------------------------------------- 
