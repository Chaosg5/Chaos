------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RolesInDepartments 
-- name     : FK_RolesInDepartments_Departments 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesInDepartments_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]'))
BEGIN
	ALTER TABLE [dbo].[RolesInDepartments]  WITH CHECK ADD CONSTRAINT [FK_RolesInDepartments_Departments] FOREIGN KEY([DepartmentId])
	REFERENCES [dbo].[Departments] ([DepartmentId])

END 

GO

------------------------------------- 
