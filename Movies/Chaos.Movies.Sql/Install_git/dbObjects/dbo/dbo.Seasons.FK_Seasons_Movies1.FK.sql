------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Seasons 
-- name     : FK_Seasons_Movies1 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Seasons_Movies1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Seasons]'))
BEGIN
	ALTER TABLE [dbo].[Seasons]  WITH CHECK ADD CONSTRAINT [FK_Seasons_Movies1] FOREIGN KEY([MovieId])
	REFERENCES [dbo].[Movies] ([MovieId])

END 

GO

------------------------------------- 
