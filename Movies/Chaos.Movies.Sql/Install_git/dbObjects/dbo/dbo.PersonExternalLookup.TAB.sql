------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : PersonExternalLookup 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonExternalLookup]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[PersonExternalLookup](
		[PersonId] [int] NOT NULL,
		[ExternalSourceId] [int] NOT NULL,
		[ExternalId] [varchar](50) NOT NULL,
	 CONSTRAINT [PK_PersonExternalLookup] PRIMARY KEY CLUSTERED 
	(
		[PersonId] ASC,
		[ExternalSourceId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonExternalLookup_ExternalSources]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[PersonExternalLookup] CHECK CONSTRAINT [FK_PersonExternalLookup_ExternalSources]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonExternalLookup_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[PersonExternalLookup] CHECK CONSTRAINT [FK_PersonExternalLookup_People]
END 

GO

------------------------------------- 
