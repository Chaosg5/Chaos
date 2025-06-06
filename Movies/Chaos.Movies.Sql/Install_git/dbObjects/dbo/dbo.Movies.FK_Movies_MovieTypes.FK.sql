------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Movies 
-- name     : FK_Movies_MovieTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Movies_MovieTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Movies]'))
BEGIN
	ALTER TABLE [dbo].[Movies]  WITH CHECK ADD CONSTRAINT [FK_Movies_MovieTypes] FOREIGN KEY([MovieTypeId])
	REFERENCES [dbo].[MovieTypes] ([MovieTypeId])

END 

GO

------------------------------------- 
