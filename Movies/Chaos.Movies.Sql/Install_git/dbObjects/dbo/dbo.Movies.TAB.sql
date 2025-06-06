------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Movies 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Movies]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Movies](
		[MovieId] [int] IDENTITY(1,1) NOT NULL,
		[MovieTypeId] [int] NOT NULL,
		[Year] [date] NOT NULL,
		[EndYear] [date] NULL,
		[RunTime] [int] NOT NULL,
	 CONSTRAINT [PK_Movies] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Movies_MovieTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Movies]'))
BEGIN
	ALTER TABLE [dbo].[Movies] CHECK CONSTRAINT [FK_Movies_MovieTypes]
END 

GO

------------------------------------- 
