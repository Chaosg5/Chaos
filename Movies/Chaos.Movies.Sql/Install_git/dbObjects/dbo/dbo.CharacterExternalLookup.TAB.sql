------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : CharacterExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CharacterExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[CharacterExternalLookup](
		[CharacterId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_CharacterExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[CharacterId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharacterExternalLookup_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharacterExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[CharacterExternalLookup] CHECK CONSTRAINT [FK_CharacterExternalLookup_Characters]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharacterExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharacterExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[CharacterExternalLookup] CHECK CONSTRAINT [FK_CharacterExternalLookup_ExternalSources]
END 

GO

------------------------------------- 
