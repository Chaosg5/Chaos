------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RolesInDepartments 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RolesInDepartments](
		[RoleInDepartmentId] [int] IDENTITY(1,1) NOT NULL,
		[RoleId] [int] NOT NULL,
		[DepartmentId] [int] NOT NULL,
	 CONSTRAINT [PK_RolesInDepartments] PRIMARY KEY CLUSTERED 
	(
		[RoleInDepartmentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_RolesInDepartments] UNIQUE NONCLUSTERED 
	(
		[DepartmentId] ASC,
		[RoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesInDepartments_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]'))
BEGIN
	ALTER TABLE [dbo].[RolesInDepartments] CHECK CONSTRAINT [FK_RolesInDepartments_Departments]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RolesInDepartments_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[RolesInDepartments]'))
BEGIN
	ALTER TABLE [dbo].[RolesInDepartments] CHECK CONSTRAINT [FK_RolesInDepartments_Roles]
END 

GO

------------------------------------- 
