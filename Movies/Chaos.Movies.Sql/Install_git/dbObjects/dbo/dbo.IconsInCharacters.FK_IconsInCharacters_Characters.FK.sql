------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInCharacters 
-- name     : FK_IconsInCharacters_Characters 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInCharacters_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInCharacters]'))
BEGIN
	ALTER TABLE [dbo].[IconsInCharacters]  WITH CHECK ADD CONSTRAINT [FK_IconsInCharacters_Characters] FOREIGN KEY([CharacterId])
	REFERENCES [dbo].[Characters] ([CharacterId])

END 

GO

------------------------------------- 
