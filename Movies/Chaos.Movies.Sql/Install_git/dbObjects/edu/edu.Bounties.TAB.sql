------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : Bounties 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[Bounties]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[Bounties](
		[BountyId] [int] IDENTITY(1,1) NOT NULL,
		[BountyTypeId] [int] NOT NULL,
		[BountyValue] [nvarchar](100) NOT NULL,
	 CONSTRAINT [PK_Bounties] PRIMARY KEY CLUSTERED 
	(
		[BountyId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[edu].[FK_Bounties_BountyTypes]') AND parent_object_id = OBJECT_ID(N'[edu].[Bounties]'))
BEGIN
	ALTER TABLE [edu].[Bounties] CHECK CONSTRAINT [FK_Bounties_BountyTypes]
END 

GO

------------------------------------- 
