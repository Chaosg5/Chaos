------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : BoxOfficeTypeTitles 
-- name     : FK_BoxOfficeTypeTitles_BoxOfficeTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOfficeTypeTitles_BoxOfficeTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOfficeTypeTitles]'))
BEGIN
	ALTER TABLE [dbo].[BoxOfficeTypeTitles]  WITH CHECK ADD CONSTRAINT [FK_BoxOfficeTypeTitles_BoxOfficeTypes] FOREIGN KEY([BoxOfficeTypeId])
	REFERENCES [dbo].[BoxOfficeTypes] ([BoxOfficeTypeId])

END 

GO

------------------------------------- 
