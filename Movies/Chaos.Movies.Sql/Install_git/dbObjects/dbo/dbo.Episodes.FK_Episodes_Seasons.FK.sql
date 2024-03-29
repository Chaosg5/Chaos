------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Episodes 
-- name     : FK_Episodes_Seasons 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Episodes_Seasons]') AND parent_object_id = OBJECT_ID(N'[dbo].[Episodes]'))
BEGIN
	ALTER TABLE [dbo].[Episodes]  WITH CHECK ADD CONSTRAINT [FK_Episodes_Seasons] FOREIGN KEY([SeasonId])
	REFERENCES [dbo].[Seasons] ([SeasonId])

END 

GO

------------------------------------- 
