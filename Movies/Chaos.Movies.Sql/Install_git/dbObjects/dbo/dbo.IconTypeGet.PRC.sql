------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconTypeGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.IconTypeGet
-- Date: 2018-04-10
-- Release: 1.0
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconTypeGet]
	@iconTypeIds dbo.IdCollection readonly
as
begin
	select t.IconTypeId
	from dbo.IconTypes as t
	inner join @iconTypeIds as i on i.Id = t.IconTypeId;

	select t.IconTypeId
		,ti.Language
		,ti.Title
	from dbo.IconTypes t
	inner join dbo.IconTypeTitles as ti on ti.IconTypeId = t.IconTypeId
	inner join @iconTypeIds as i on i.Id = t.IconTypeId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconTypeGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconTypeGet] 
GRANT EXECUTE ON [dbo].[IconTypeGet] TO [rCMDB] 
GO

------------------------------------- 
