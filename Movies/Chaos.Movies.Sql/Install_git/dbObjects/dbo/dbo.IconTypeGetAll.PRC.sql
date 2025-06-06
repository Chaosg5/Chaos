------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypeGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.IconTypeGetAll
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconTypeGetAll]
as
begin
	select t.IconTypeId
	from dbo.IconTypes as t;

	select t.IconTypeId
		,ti.Language
		,ti.Title
	from dbo.IconTypes t
	inner join dbo.IconTypeTitles as ti on ti.IconTypeId = t.IconTypeId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypeGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconTypeGetAll] 
GRANT EXECUTE ON [dbo].[IconTypeGetAll] TO [rCMDB] 
GO

------------------------------------- 
