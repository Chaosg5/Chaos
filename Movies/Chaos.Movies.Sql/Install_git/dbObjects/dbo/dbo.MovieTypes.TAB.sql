------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieTypes](
		[MovieTypeId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_MovieTypes] PRIMARY KEY CLUSTERED 
	(
		[MovieTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
