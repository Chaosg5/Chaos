------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RoleTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RoleTitles](
		[RoleId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_RoleTitles] PRIMARY KEY CLUSTERED 
	(
		[RoleId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles] CHECK CONSTRAINT [FK_RoleTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Roles]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles] CHECK CONSTRAINT [FK_RoleTitles_Roles]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Roles1]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles] CHECK CONSTRAINT [FK_RoleTitles_Roles1]
END 

GO

------------------------------------- 
