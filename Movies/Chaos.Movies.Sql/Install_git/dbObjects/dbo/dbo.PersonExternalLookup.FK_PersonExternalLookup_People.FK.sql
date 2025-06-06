------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : PersonExternalLookup 
-- name     : FK_PersonExternalLookup_People 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PersonExternalLookup_People]') AND parent_object_id = OBJECT_ID(N'[dbo].[PersonExternalLookup]'))
BEGIN
	ALTER TABLE [dbo].[PersonExternalLookup]  WITH CHECK ADD CONSTRAINT [FK_PersonExternalLookup_People] FOREIGN KEY([PersonId])
	REFERENCES [dbo].[People] ([PersonId])

END 

GO

------------------------------------- 
