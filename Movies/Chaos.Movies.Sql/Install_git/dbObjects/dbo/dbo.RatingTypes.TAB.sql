------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[RatingTypes](
		[RatingTypeId] [int] IDENTITY(1,1) NOT NULL,
		[ParentRatingTypeId] [int] NULL,
	 CONSTRAINT [PK_RatingTypes] PRIMARY KEY CLUSTERED 
	(
		[RatingTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RatingTypes_RatingTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[RatingTypes]'))
BEGIN
	ALTER TABLE [dbo].[RatingTypes] CHECK CONSTRAINT [FK_RatingTypes_RatingTypes]
END 

GO

------------------------------------- 
