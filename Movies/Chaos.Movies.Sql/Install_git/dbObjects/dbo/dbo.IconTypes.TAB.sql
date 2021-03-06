------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypes 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypes]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[IconTypes](
		[IconTypeId] [int] IDENTITY(1,1) NOT NULL,
	 CONSTRAINT [PK_IconTypes] PRIMARY KEY CLUSTERED 
	(
		[IconTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO

------------------------------------- 
