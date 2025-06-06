------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : FavoriteTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FavoriteTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[FavoriteTypeTitles](
		[FavoriteTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_FavoriteTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[FavoriteTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FavoriteTypeTitles_FavoriteTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[FavoriteTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[FavoriteTypeTitles] CHECK CONSTRAINT [FK_FavoriteTypeTitles_FavoriteTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_FavoriteTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[FavoriteTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[FavoriteTypeTitles] CHECK CONSTRAINT [FK_FavoriteTypeTitles_Languages]
END 

GO

------------------------------------- 
