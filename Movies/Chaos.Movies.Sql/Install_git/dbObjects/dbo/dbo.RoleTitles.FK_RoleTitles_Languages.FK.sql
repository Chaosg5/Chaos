------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RoleTitles 
-- name     : FK_RoleTitles_Languages 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RoleTitles_Languages]') AND parent_object_id = OBJECT_ID(N'[dbo].[RoleTitles]'))
BEGIN
	ALTER TABLE [dbo].[RoleTitles]  WITH CHECK ADD CONSTRAINT [FK_RoleTitles_Languages] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
