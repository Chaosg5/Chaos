------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : TaskBounties 
-- name     : FK_TaskBounties_Bounties 
-- Type     : foreignkey 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_TaskBounties_Bounties]') AND parent_object_id = OBJECT_ID(N'[edu].[TaskBounties]'))
BEGIN
	ALTER TABLE [edu].[TaskBounties]  WITH CHECK ADD CONSTRAINT [FK_TaskBounties_Bounties] FOREIGN KEY([BountyId])
	REFERENCES [edu].[Bounties] ([BountyId])

END 

GO

------------------------------------- 
