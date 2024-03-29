------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingTypeGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingTypesGet
-- Date: 2018-04-12
-- Release: 1.0
-- Summary:
--   * Gets the specified rating types.
-- Param:
--   @ratingTypeIds dbo.IdCollection
--     * The list of ids of the rating types to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingTypeGet]
	@ratingTypeIds dbo.IdCollection readonly
as
begin
	select t.RatingTypeId
	from dbo.RatingTypes t
	inner join @ratingTypeIds as i on i.Id = t.RatingTypeId

	select t.RatingTypeId
		,ti.Language
		,ti.Title
	from dbo.RatingTypes t
	inner join dbo.RatingTypeTitles as ti on ti.RatingTypeId = t.RatingTypeId
	inner join @ratingTypeIds as i on i.Id = t.RatingTypeId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingTypeGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingTypeGet] 
GRANT EXECUTE ON [dbo].[RatingTypeGet] TO [rCMDB] 
GO

------------------------------------- 
