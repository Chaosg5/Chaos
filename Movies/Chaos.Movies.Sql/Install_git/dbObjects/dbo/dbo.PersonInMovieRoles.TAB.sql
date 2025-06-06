------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PersonInMovieRoles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonInMovieRoles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PersonInMovieRoles](
		[PersonInMovieRoleId] [int] IDENTITY(1,1) NOT NULL,
		[PersonInMovieId] [int] NOT NULL,
		[RoleInDepartmentId] [int] NOT NULL,
	 CONSTRAINT [PK_PersonInMovieRoles] PRIMARY KEY CLUSTERED 
	(
		[PersonInMovieRoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UK_PersonInMovieRoles] UNIQUE NONCLUSTERED 
	(
		[PersonInMovieId] ASC,
		[RoleInDepartmentId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonInMovieRoles_PeopleInMovies]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonInMovieRoles]'))
BEGIN
	ALTER TABLE [dbo].[PersonInMovieRoles] CHECK CONSTRAINT [FK_PersonInMovieRoles_PeopleInMovies]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonInMovieRoles_RolesInDepartments]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonInMovieRoles]'))
BEGIN
	ALTER TABLE [dbo].[PersonInMovieRoles] CHECK CONSTRAINT [FK_PersonInMovieRoles_RolesInDepartments]
END 

GO

------------------------------------- 
