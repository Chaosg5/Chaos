------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : Acquisitions 
-- name     : FK_Acquisitions_MoviesOwnedByUser 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Acquisitions_MoviesOwnedByUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[Acquisitions]'))
BEGIN
	ALTER TABLE [dbo].[Acquisitions]  WITH CHECK ADD CONSTRAINT [FK_Acquisitions_MoviesOwnedByUser] FOREIGN KEY([MoviesOwnedByUserId])
	REFERENCES [dbo].[MoviesOwnedByUser] ([MoviesOwnedByUserId])

END 

GO

------------------------------------- 
