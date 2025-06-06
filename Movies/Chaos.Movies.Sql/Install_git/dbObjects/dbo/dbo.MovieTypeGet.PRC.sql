------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : MovieTypeGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.MovieTypesGet
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Gets the specified movie types.
-- Param:
--   @movieTypeIds dbo.IdCollection
--     * The list of ids of the movie types to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[MovieTypeGet]
	@movieTypeIds dbo.IdCollection readonly
as
begin
	select t.MovieTypeId
	from dbo.MovieTypes t
	inner join @movieTypeIds as i on i.Id = t.MovieTypeId

	select t.MovieTypeId
		,ti.Language
		,ti.Title
	from dbo.MovieTypes t
	inner join dbo.MovieTypeTitles as ti on ti.MovieTypeId = t.MovieTypeId
	inner join @movieTypeIds as i on i.Id = t.MovieTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MovieTypeGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[MovieTypeGet] 
GRANT EXECUTE ON [dbo].[MovieTypeGet] TO [rCMDB] 
GO

------------------------------------- 
