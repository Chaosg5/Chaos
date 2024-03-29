------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Hashtags 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Hashtags]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Hashtags](
		[Hashtag] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_Hashtags] PRIMARY KEY CLUSTERED 
	(
		[Hashtag] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
