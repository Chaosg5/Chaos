------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : Acquisitions 
-- Type     : table 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Acquisitions]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Acquisitions](
		[AcquisitionId] [int] IDENTITY(1,1) NOT NULL,
		[MoviesOwnedByUserId] [int] NOT NULL,
		[AcquiredAt] [date] NULL,
		[DateUncertain] [bit] NOT NULL,
		[VendorId] [int] NULL,
		[Price] [float] NOT NULL,
	 CONSTRAINT [PK_Acquisitions] PRIMARY KEY CLUSTERED 
	(
		[AcquisitionId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END 

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DF_Acquisitions_DateUncertain]') AND type = 'D')
BEGIN
	ALTER TABLE [dbo].[Acquisitions] ADD  CONSTRAINT [DF_Acquisitions_DateUncertain]  DEFAULT ((0)) FOR [DateUncertain]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Acquisitions_MoviesOwnedByUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[Acquisitions]'))
BEGIN
	ALTER TABLE [dbo].[Acquisitions] CHECK CONSTRAINT [FK_Acquisitions_MoviesOwnedByUser]
END 

GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Acquisitions_Vendors]') AND parent_object_id = OBJECT_ID(N'[dbo].[Acquisitions]'))
BEGIN
	ALTER TABLE [dbo].[Acquisitions] CHECK CONSTRAINT [FK_Acquisitions_Vendors]
END 

GO

------------------------------------- 
