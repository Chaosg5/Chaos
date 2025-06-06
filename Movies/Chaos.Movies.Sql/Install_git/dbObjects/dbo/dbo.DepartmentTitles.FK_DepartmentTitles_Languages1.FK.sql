------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : DepartmentTitles 
-- name     : FK_DepartmentTitles_Languages1 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DepartmentTitles_Languages1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DepartmentTitles]'))
BEGIN
	ALTER TABLE [dbo].[DepartmentTitles]  WITH CHECK ADD CONSTRAINT [FK_DepartmentTitles_Languages1] FOREIGN KEY([Language])
	REFERENCES [dbo].[Languages] ([Language])

END 

GO

------------------------------------- 
