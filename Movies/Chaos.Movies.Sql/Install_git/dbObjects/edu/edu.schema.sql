------------------------------------- 
-- database : CMDB 
-- scheme   : edu 
-- name     : edu 
-- Type     : schema 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'edu')
BEGIN
	EXEC sys.sp_executesql N'CREATE SCHEMA [edu]'

END 

GO

------------------------------------- 
