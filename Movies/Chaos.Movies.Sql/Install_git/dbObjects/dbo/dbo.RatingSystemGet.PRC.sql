------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemGet 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingSystemsGet
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Gets the specified rating systems.
-- Param:
--   @ratingSystemIds dbo.IdCollection
--     * The list of ids of the rating systems to get.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingSystemGet]
	@ratingSystemIds dbo.IdCollection readonly
as
begin
	select s.RatingSystemId
	from dbo.RatingSystems s
	inner join @ratingSystemIds as i on i.Id = s.RatingSystemId;

	select s.RatingSystemId
		,ti.Language
		,ti.Title
	from dbo.RatingSystems s
	inner join dbo.RatingSystemTitles as ti on ti.RatingSystemId = s.RatingSystemId
	inner join @ratingSystemIds as i on i.Id = s.RatingSystemId;

	select s.RatingSystemId
		,sv.RatingTypeId
		,sv.Weight
	from dbo.RatingSystems s
	inner join dbo.RatingSystemValues as sv on sv.RatingSystemId = s.RatingSystemId
	inner join @ratingSystemIds as i on i.Id = s.RatingSystemId;
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemGet]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingSystemGet] 
GRANT EXECUTE ON [dbo].[RatingSystemGet] TO [rCMDB] 
GO

------------------------------------- 
