------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconTypeTitles](
		[IconTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_IconTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[IconTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconTypeTitles_IconTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[IconTypeTitles] CHECK CONSTRAINT [FK_IconTypeTitles_IconTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[IconTypeTitles] CHECK CONSTRAINT [FK_IconTypeTitles_Languages]
END 

GO

------------------------------------- 
