------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RolesInDepartments 
-- name     : FK_RolesInDepartments_Roles 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesInDepartments_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]'))
BEGIN
	ALTER TABLE [dbo].[RolesInDepartments]  WITH CHECK ADD CONSTRAINT [FK_RolesInDepartments_Roles] FOREIGN KEY([RoleId])
	REFERENCES [dbo].[Roles] ([RoleId])

END 

GO

------------------------------------- 
