------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : FavoriteTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FavoriteTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[FavoriteTypes](
		[FavoriteTypeId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_FavoriteTypes] PRIMARY KEY CLUSTERED 
	(
		[FavoriteTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
