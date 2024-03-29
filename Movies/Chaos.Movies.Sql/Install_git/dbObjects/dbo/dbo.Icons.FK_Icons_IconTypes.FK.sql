------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Icons 
-- name     : FK_Icons_IconTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Icons_IconTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Icons]'))
BEGIN
	ALTER TABLE [dbo].[Icons]  WITH CHECK ADD CONSTRAINT [FK_Icons_IconTypes] FOREIGN KEY([IconTypeId])
	REFERENCES [dbo].[IconTypes] ([IconTypeId])

END 

GO

------------------------------------- 
