------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieSeriesTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieSeriesTypes](
		[MovieSeriesTypeId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_CollectionTypes] PRIMARY KEY CLUSTERED 
	(
		[MovieSeriesTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
