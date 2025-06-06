------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GenreTitles 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenreTitles]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[GenreTitles](
		[GenreId] [int] NOT NULL,
		[Language] [varchar](8) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_GenreTitles] PRIMARY KEY CLUSTERED 
	(
		[GenreId] ASC,
		[Language] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreTitles_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreTitles]'))
BEGIN
	ALTER TABLE [dbo].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Genres]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreTitles]'))
BEGIN
	ALTER TABLE [dbo].[GenreTitles] CHECK CONSTRAINT [FK_GenreTitles_Languages]
END 

GO

------------------------------------- 
