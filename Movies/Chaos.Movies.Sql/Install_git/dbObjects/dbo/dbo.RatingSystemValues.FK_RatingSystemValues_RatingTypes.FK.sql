------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingSystemValues 
-- name     : FK_RatingSystemValues_RatingTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingSystemValues_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingSystemValues]'))
BEGIN
	ALTER TABLE [dbo].[RatingSystemValues]  WITH CHECK ADD CONSTRAINT [FK_RatingSystemValues_RatingTypes] FOREIGN KEY([RatingTypeId])
	REFERENCES [dbo].[RatingTypes] ([RatingTypeId])

END 

GO

------------------------------------- 
