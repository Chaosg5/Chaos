------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : IconGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.IconGet
-- Date: 2018-03-19
-- Release: 1.0
-- Summary:
--   * Gets the specified icon.
-- Param:
--   @iconIds dbo.IdCollection
--     * The list of ids of the icons to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[IconGet]
	@iconIds dbo.IdCollection readonly
as
begin
	select e.IconId
		,e.IconTypeId
		,e.IconUrl
		,e."Data"
		,e.DataSize
	from dbo.Icons e
	inner join @iconIds as i on i.Id = e.IconId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[IconGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[IconGet] 
GRANT EXECUTE ON [dbo].[IconGet] TO [rCMDB] 
GO

------------------------------------- 
