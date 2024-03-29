------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : Bounties 
-- name     : FK_Bounties_BountyTypes 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Bounties_BountyTypes]') AND parent_object_id = OBJECT_ID(N'[edu].[Bounties]'))
BEGIN
	ALTER TABLE [edu].[Bounties]  WITH CHECK ADD CONSTRAINT [FK_Bounties_BountyTypes] FOREIGN KEY([BountyTypeId])
	REFERENCES [edu].[BountyTypes] ([BountyTypeId])

END 

GO

------------------------------------- 
