------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- parent   : RatingTypes 
-- name     : FK_RatingTypes_RatingTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypes_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypes]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypes]  WITH CHECK ADD CONSTRAINT [FK_RatingTypes_RatingTypes] FOREIGN KEY([ParentRatingTypeId])
	REFERENCES [dbo].[RatingTypes] ([RatingTypeId])

END 

GO

------------------------------------- 
