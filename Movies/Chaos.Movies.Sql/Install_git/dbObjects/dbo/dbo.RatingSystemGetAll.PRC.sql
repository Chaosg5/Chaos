------------------------------------- 
-- database : CMDB 
-- scheme   : dbo 
-- name     : RatingSystemGetAll 
-- Type     : storedprocedure 
------------------------------------- 
SET ANSI_NULLS ON 
GO 
SET QUOTED_IDENTIFIER ON 
GO 

DECLARE @sql as nvarchar(max) = N'

------------------------------------------------------------------------
-- Name: dbo.RatingSystemGetAll
-- Date: 2018-04-11
-- Release: 1.0
-- Summary:
--   * Gets all rating systems.
------------------------------------------------------------------------
-- Copyright (c) Erik Bunnstad. All rights reserved.
------------------------------------------------------------------------

CREATE PROCEDURE [dbo].[RatingSystemGetAll]
as
begin
	select s.RatingSystemId
	from dbo.RatingSystems s

	select s.RatingSystemId
		,ti.Language
		,ti.Title
	from dbo.RatingSystems s
	inner join dbo.RatingSystemTitles as ti on ti.RatingSystemId = s.RatingSystemId

	select s.RatingSystemId
		,sv.RatingTypeId
		,sv.Weight
	from dbo.RatingSystems s
	inner join dbo.RatingSystemValues as sv on sv.RatingSystemId = s.RatingSystemId
end;

' 

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RatingSystemGetAll]') AND type in (N'P', N'PC')) 
	SET @sql=replace(@sql,'CREATE PROCEDURE','ALTER PROCEDURE') 

EXEC dbo.sp_executesql @sql 

GO

-- Grants rCMDB EXECUTE rights to [dbo].[RatingSystemGetAll] 
GRANT EXECUTE ON [dbo].[RatingSystemGetAll] TO [rCMDB] 
GO

------------------------------------- 
