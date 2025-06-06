------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypeTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypeTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RatingTypeTitles](
		[RatingTypeId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[Description] [nvarchar](500) NULL,
	 CONSTRAINT [PK_RatingTypeTitles] PRIMARY KEY CLUSTERED 
	(
		[RatingTypeId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypeTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypeTitles] CHECK CONSTRAINT [FK_RatingTypeTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypeTitles_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypeTitles] CHECK CONSTRAINT [FK_RatingTypeTitles_RatingTypes]
END 

GO

------------------------------------- 
