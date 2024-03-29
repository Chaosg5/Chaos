------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- parent   : TaskBounties 
-- name     : UX_TaskBounties 
-- Type     : nonuniqueindex 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[edu].[TaskBounties]') AND name = N'UX_TaskBounties')
BEGIN
	CREATE NONCLUSTERED INDEX [UX_TaskBounties] ON [edu].[TaskBounties]
	(
		[TaskId] ASC,
		[BountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

END 

GO

------------------------------------- 
