------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : DepartmentTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[DepartmentTitles](
		[DepartmentId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_DepartmentTitles] PRIMARY KEY CLUSTERED 
	(
		[DepartmentId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Departments]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles] CHECK CONSTRAINT [FK_DepartmentTitles_Departments]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles] CHECK CONSTRAINT [FK_DepartmentTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Languages1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles] CHECK CONSTRAINT [FK_DepartmentTitles_Languages1]
END 

GO

------------------------------------- 
