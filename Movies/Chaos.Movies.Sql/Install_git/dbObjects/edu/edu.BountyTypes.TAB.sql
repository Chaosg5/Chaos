------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : BountyTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[edu].[BountyTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [edu].[BountyTypes](
		[BountyTypeId] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](50) NOT NULL,
	 CONSTRAINT [PK_BountyTypes] PRIMARY KEY CLUSTERED 
	(
		[BountyTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
