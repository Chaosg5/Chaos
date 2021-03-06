------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : BoxOffice 
-- name     : FK_BoxOffice_BoxOfficeTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOffice_BoxOfficeTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOffice]'))
BEGIN
	ALTER TABLE [dbo].[BoxOffice]  WITH CHECK ADD CONSTRAINT [FK_BoxOffice_BoxOfficeTypes] FOREIGN KEY([BoxOfficeTypeId])
	REFERENCES [dbo].[BoxOfficeTypes] ([BoxOfficeTypeId])

END 

GO

------------------------------------- 
