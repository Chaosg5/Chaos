------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : WatchTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WatchTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[WatchTypes](
		[WatchTypeId] [int] IDENTITY(1,1) NOT NULL,
		[Name] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_WatchTypes] PRIMARY KEY CLUSTERED 
	(
		[WatchTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
