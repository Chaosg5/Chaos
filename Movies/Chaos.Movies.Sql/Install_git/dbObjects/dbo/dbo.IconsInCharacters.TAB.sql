------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconsInCharacters 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconsInCharacters]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconsInCharacters](
		[IconId] [int] NOT NULL,
		[CharacterId] [int] NOT NULL,
		[Order] [int] NOT NULL,
	 CONSTRAINT [PK_IconsInCharacters] PRIMARY KEY CLUSTERED 
	(
		[IconId] ASC,
		[CharacterId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCharacters_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInCharacters]'))
BEGIN
	ALTER TABLE [dbo].[IconsInCharacters] CHECK CONSTRAINT [FK_IconsInCharacters_Characters]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCharacters_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInCharacters]'))
BEGIN
	ALTER TABLE [dbo].[IconsInCharacters] CHECK CONSTRAINT [FK_IconsInCharacters_Icons]
END 

GO

------------------------------------- 
