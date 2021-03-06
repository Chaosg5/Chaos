------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : IconTypeTitles 
-- name     : FK_IconTypeTitles_IconTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconTypeTitles_IconTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[IconTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_IconTypeTitles_IconTypes] FOREIGN KEY([IconTypeId])
	REFERENCES [dbo].[IconTypes] ([IconTypeId])

END 

GO

------------------------------------- 
