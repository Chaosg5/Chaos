------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieSeriesTitles](
		[MovieSeriesId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_CollectionTitles] PRIMARY KEY CLUSTERED 
	(
		[MovieSeriesId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CollectionTitles_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles] CHECK CONSTRAINT [FK_CollectionTitles_Collections]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles] CHECK CONSTRAINT [FK_MovieSeriesTitles_Languages]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieSeriesTitles_MovieSeriesTitles]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeriesTitles]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeriesTitles] CHECK CONSTRAINT [FK_MovieSeriesTitles_MovieSeriesTitles]
END 

GO

------------------------------------- 
