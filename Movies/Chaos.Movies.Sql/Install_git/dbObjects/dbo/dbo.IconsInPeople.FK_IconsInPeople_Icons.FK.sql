------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconsInPeople 
-- name     : FK_IconsInPeople_Icons 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconsInPeople_Icons]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconsInPeople]'))
BEGIN
	ALTER TABLE [dbo].[IconsInPeople]  WITH CHECK ADD CONSTRAINT [FK_IconsInPeople_Icons] FOREIGN KEY([IconId])
	REFERENCES [dbo].[Icons] ([IconId])

END 

GO

------------------------------------- 
