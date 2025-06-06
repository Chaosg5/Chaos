------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : GenreExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GenreExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[GenreExternalLookup](
		[GenreId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_GenreExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[GenreId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[GenreExternalLookup] CHECK CONSTRAINT [FK_GenreExternalLookup_ExternalSources]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GenreExternalLookup_Genres]') AND parent_object_id = OBJECT_ID(N'[dbo].[GenreExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[GenreExternalLookup] CHECK CONSTRAINT [FK_GenreExternalLookup_Genres]
END 

GO

------------------------------------- 
