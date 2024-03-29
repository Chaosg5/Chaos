------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : BoxOffice 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BoxOffice]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[BoxOffice](
		[MovieId] [int] NOT NULL,
		[BoxOfficeTypeId] [int] NOT NULL,
		[USD] [bigint] NOT NULL,
	 CONSTRAINT [PK_BoxOffice] PRIMARY KEY CLUSTERED 
	(
		[MovieId] ASC,
		[BoxOfficeTypeId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOffice_BoxOfficeTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOffice]'))
BEGIN
	ALTER TABLE [dbo].[BoxOffice] CHECK CONSTRAINT [FK_BoxOffice_BoxOfficeTypes]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_BoxOffice_Movies]') AND parent_object_id = OBJECT_ID(N'[dbo].[BoxOffice]'))
BEGIN
	ALTER TABLE [dbo].[BoxOffice] CHECK CONSTRAINT [FK_BoxOffice_Movies]
END 

GO

------------------------------------- 
