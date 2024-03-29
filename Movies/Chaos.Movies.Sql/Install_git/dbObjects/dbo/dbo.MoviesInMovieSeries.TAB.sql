------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MoviesInMovieSeries 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MoviesInMovieSeries]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MoviesInMovieSeries](
		[MovieId] [int] NOT NULL,
		[MovieSeriesId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_MoviesInCollection] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[MovieSeriesId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInCollection_Collections]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInMovieSeries] CHECK CONSTRAINT [FK_MoviesInCollection_Collections]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MoviesInCollection_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MoviesInMovieSeries]'))
BEGIN
	ALTER TABLE [dbo].[MoviesInMovieSeries] CHECK CONSTRAINT [FK_MoviesInCollection_Movies]
END 

GO

------------------------------------- 
