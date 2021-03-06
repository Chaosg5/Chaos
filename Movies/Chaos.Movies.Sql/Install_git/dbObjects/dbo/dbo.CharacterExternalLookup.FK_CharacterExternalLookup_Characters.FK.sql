------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : CharacterExternalLookup 
-- name     : FK_CharacterExternalLookup_Characters 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CharacterExternalLookup_Characters]') AND parent_object_id = OBJECT_ID(N'[dbo].[CharacterExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[CharacterExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_CharacterExternalLookup_Characters] FOREIGN KEY([CharacterId])
	REFERENCES [dbo].[Characters] ([CharacterId])

END 

GO

------------------------------------- 
