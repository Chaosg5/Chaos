------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : BoxOfficeTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[BoxOfficeTypeTitles](
		[BoxOfficeTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_BoxOfficeTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[BoxOfficeTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOfficeTypeTitles_BoxOfficeTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[BoxOfficeTypeTitles] CHECK CONSTRAINT [FK_BoxOfficeTypeTitles_BoxOfficeTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOfficeTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[BoxOfficeTypeTitles] CHECK CONSTRAINT [FK_BoxOfficeTypeTitles_Languages]
END 

GO

------------------------------------- 
