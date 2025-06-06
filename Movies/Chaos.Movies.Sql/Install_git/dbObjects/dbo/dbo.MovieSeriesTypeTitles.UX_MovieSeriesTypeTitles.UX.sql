------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : MovieSeriesTypeTitles 
-- name     : UX_MovieSeriesTypeTitles 
-- Type     : uniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[MovieSeriesTypeTitles]') AND name = N'UX_MovieSeriesTypeTitles')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX [UX_MovieSeriesTypeTitles] ON [dbo].[MovieSeriesTypeTitles]
	(
		[Language] ASC,
		[Title] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
