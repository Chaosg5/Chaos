------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PersonInMovieRoles 
-- name     : FK_PersonInMovieRoles_RolesInDepartments 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonInMovieRoles_RolesInDepartments]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonInMovieRoles]'))
BEGIN
	ALTER TABLE [dbo].[PersonInMovieRoles]  WITH CHECK ADD CONSTRAINT [FK_PersonInMovieRoles_RolesInDepartments] FOREIGN KEY([RoleInDepartmentId])
	REFERENCES [dbo].[RolesInDepartments] ([RoleInDepartmentId])

END 

GO

------------------------------------- 
