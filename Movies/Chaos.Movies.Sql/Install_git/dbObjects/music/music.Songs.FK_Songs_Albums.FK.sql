------------------------------------- 
-- database : CMDB 
-- scheme   : music 
-- parent   : Songs 
-- name     : FK_Songs_Albums 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[music].[FK_Songs_Albums]') AND parent_object_id = OBJECT_ID(N'[music].[Songs]'))
BEGIN
	ALTER TABLE [music].[Songs]  WITH CHECK ADD CONSTRAINT [FK_Songs_Albums] FOREIGN KEY([AlbumId])
	REFERENCES [music].[Albums] ([AlbumId])

END 

GO

------------------------------------- 
