------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeries 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeries]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieSeries](
		[MovieSeriesId] [int] IDENTITY(1,1) NOT NULL,
		[MovieSeriesTypeId] [int] NOT NULL,
	 CONSTRAINT [PK_Collections] PRIMARY KEY CLUSTERED 
	(
		[MovieSeriesId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Collections_CollectionTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MovieSeries] CHECK CONSTRAINT [FK_Collections_CollectionTypes]
END 

GO

------------------------------------- 
