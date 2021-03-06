------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MovieExternalLookup](
		[MovieId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_MovieExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalLookup] CHECK CONSTRAINT [FK_MovieExternalLookup_ExternalSources]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MovieExternalLookup_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[MovieExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[MovieExternalLookup] CHECK CONSTRAINT [FK_MovieExternalLookup_Movies]
END 

GO

------------------------------------- 
