------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- name     : Genres 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[music].[Genres]') AND type in (N'U'))
BEGIN
	CREATE TABLE [music].[Genres](
		[GenreId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_Genres] PRIMARY KEY CLUSTERED 
	(
		[GenreId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
